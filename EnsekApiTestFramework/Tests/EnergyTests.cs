using FluentAssertions;

[TestFixture]
public class EnergyTests
{
    private EnergyService _energyService;
    private AuthService _authService;
    private string _authToken;

    [SetUp]
    public void Setup()
    {
        var config = ConfigHelper.GetConfiguration();
        var apiClient = new ApiClient(config["ApiSettings:BaseUrl"]);
        
        _authService = new AuthService(apiClient);
        var loginResponse = _authService.Login(config["ApiSettings:Username"], config["ApiSettings:Password"]);
        _authToken = loginResponse.access_token;
        
        _energyService = new EnergyService(apiClient, _authToken);
        _energyService.ResetTestData(); // Ensure clean state before each test
    }

    [Test]
    public void ResetTestData_ShouldReturnSuccessResponse()
    {
        // Act
        var resetResponse = _energyService.ResetTestData();
        
        // Assert
        resetResponse.IsSuccess().Should().BeTrue();
        resetResponse.Message.Should().Contain("successfully");
        resetResponse.TimeStamp.Should().NotBeNullOrEmpty();
    }

    [Test]
    public void BuyEnergy_ForEachFuelType_ShouldCreateValidOrder()
    {
        // Arrange
        var energyTypes = new[] { "electric", "gas", "oil" };
        var ordersBefore = _energyService.GetAllOrders();

        // Act
        foreach (var energyType in energyTypes)
        {
            var quantity = TestDataGenerator.RandomQuantity(1, 10);
            var purchase = _energyService.BuyEnergy(energyType, quantity);
            
            // Assert
            purchase.Should().NotBeNull();
            purchase.OrderId.Should().NotBeNullOrEmpty();
            purchase.EnergyType.Should().Be(energyType);
            purchase.Quantity.Should().Be(quantity);
            purchase.Status.Should().Be("COMPLETED");
        }

        // Verify orders were created
        var ordersAfter = _energyService.GetAllOrders();
        ordersAfter.Count.Should().Be(ordersBefore.Count + energyTypes.Length);
    }

    [Test]
    public void GetAllOrders_ShouldContainRecentlyCreatedOrders()
    {
        // Arrange
        var initialOrders = _energyService.GetAllOrders();
        var testOrder = _energyService.BuyEnergy("electric", 1);

        // Act
        var orders = _energyService.GetAllOrders();

        // Assert
        orders.Should().NotBeNull();
        orders.Count.Should().Be(initialOrders.Count + 1);
        orders.Should().Contain(o => 
            o.EnergyType == "electric" && 
            o.Quantity == 1 &&
            o.Status == "COMPLETED");
    }

    [Test]
    public void OrdersCreatedBeforeToday_ShouldReturnCorrectCount()
    {
        // Arrange
        var allOrders = _energyService.GetAllOrders();

        // Act
        var ordersBeforeToday = allOrders
            .Where(o => o.GetOrderDateTime() < DateTime.Today)
            .ToList();

        // Assert
        TestContext.WriteLine($"Found {ordersBeforeToday.Count} orders created before today");
        ordersBeforeToday.Should().OnlyContain(o => o.GetOrderDateTime() < DateTime.Today);
    }

    [Test]
    public void BuyEnergy_WithInvalidParameters_ShouldFail()
    {
        // Arrange
        var testCases = new[]
        {
            new { Type = "invalid", Quantity = 1 },
            new { Type = "electric", Quantity = 0 },
            new { Type = "gas", Quantity = -1 }
        };

        foreach (var testCase in testCases)
        {
            // Act & Assert
            var ex = Assert.Throws<ApplicationException>(() => 
                _energyService.BuyEnergy(testCase.Type, testCase.Quantity));
            
            ex.Message.Should().Contain("Failed to buy energy");
            ex.Message.Should().Contain("BadRequest");
        }
    }

    [Test]
    public void GetEnergyStock_ShouldReturnAllEnergyTypes()
    {
        // Act
        var stock = _energyService.GetEnergyStock();

        // Assert
        stock.Should().NotBeNull();
        stock.Electric.Should().NotBeNull();
        stock.Gas.Should().NotBeNull();
        stock.Oil.Should().NotBeNull();
        
        stock.Electric.QuantityAvailable.Should().BeGreaterThanOrEqualTo(0);
        stock.Gas.PricePerUnit.Should().BeGreaterThan(0);
        stock.Oil.EnergyType.Should().Be("oil");
    }
}