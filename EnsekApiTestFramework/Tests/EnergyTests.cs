using FluentAssertions;
using EnsekApiTestFramework.Models;
using Newtonsoft.Json;

[TestFixture]
public class EnergyTests
{
    private EnergyService _energyService;
    private AuthService _authService;
    private string _authToken;
    private ApiClient _apiClient;

    [SetUp]
    public void Setup()
    {
        // Use ConfigHelper static properties directly
        _apiClient = new ApiClient(ConfigHelper.BaseUrl);
        
        _authService = new AuthService(_apiClient);
        var loginResponse = _authService.Login(ConfigHelper.Username, ConfigHelper.Password);
        _authToken = loginResponse.AccessToken;
        
        _energyService = new EnergyService(_apiClient, _authToken);
        
        // Reset with status verification
        var resetResponse = _energyService.ResetTestData();
        resetResponse.Should().NotBeNull();
        //resetResponse.IsSuccess().Should().BeTrue("Test data reset must succeed before each test");
    }

    [Test]
    public void ResetTestData_ShouldReturnSuccessResponse()
    {
        // Act
        var resetResponse = _energyService.ResetTestData();
        
        // Assert
        resetResponse.Should().NotBeNull();
        resetResponse.Message.Should().Contain("successfully");
        resetResponse.TimeStamp.Should().NotBeNullOrEmpty();
        DateTime.Parse(resetResponse.TimeStamp).Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(5));
    }

    [Test]
    public void BuyEnergy_ForEachFuelType_ShouldCreateValidOrder()
    {
        // Arrange
        var stock = _energyService.GetEnergyStock();
        var ordersBefore = _energyService.GetAllOrders().Orders;
        
        // Use actual energy IDs from stock
        var energyIds = new[] { stock.Electric.EnergyId, stock.Gas.EnergyId, stock.Oil.EnergyId };

        // Act & Assert
        foreach (var energyId in energyIds)
        {
            var quantity = 1; // Safe quantity
            var purchase = _energyService.BuyEnergy(energyId, quantity);
            
            // Assert
            purchase.Should().NotBeNull();
            purchase.OrderId.Should().NotBeNullOrEmpty();
            purchase.EnergyId.Should().Be(energyId);
            purchase.Quantity.Should().Be(quantity);
            purchase.Status.Should().Be("COMPLETED");

            // Verify order exists in system
            var fetchedOrder = _energyService.GetOrderById(purchase.OrderId);
            fetchedOrder.Should().BeEquivalentTo(purchase, "Created order should match fetched order");
        }

        // Final count verification
        var ordersAfter = _energyService.GetAllOrders();
        ordersAfter.Orders.Count.Should().Be(ordersBefore.Count + energyIds.Length);
        ordersAfter.TotalOrders.Should().Be(ordersAfter.Orders.Count);
    }

    [Test]
    public void GetAllOrders_ShouldContainRecentlyCreatedOrders()
    {
        // Arrange
        var electricId = _energyService.GetEnergyStock().Electric.EnergyId;
        var testOrder = _energyService.BuyEnergy(electricId, 1);

        // Act
        var ordersResponse = _energyService.GetAllOrders();

        // Assert
        ordersResponse.Should().NotBeNull();
        ordersResponse.Orders.Should().ContainEquivalentOf(new 
        {
            OrderId = testOrder.OrderId,
            EnergyId = electricId,
            Quantity = 1,
            Status = "COMPLETED"
        });
    }

    [Test]
    public void GetOrderById_ShouldReturnCorrectOrderDetails()
    {
        // Arrange
        var gasId = _energyService.GetEnergyStock().Gas.EnergyId;
        var createdOrder = _energyService.BuyEnergy(gasId, 5);

        // Act
        var fetchedOrder = _energyService.GetOrderById(createdOrder.OrderId);

        // Assert
        fetchedOrder.Should().BeEquivalentTo(createdOrder);
        fetchedOrder.TimeCreated.Should().NotBeNullOrWhiteSpace();
        DateTime.Parse(fetchedOrder.TimeCreated).Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(5));
    }

    [Test]
    public void BuyEnergy_WithInvalidParameters_ShouldThrowProperError()
    {
        // Arrange
        var stock = _energyService.GetEnergyStock();
        var validId = stock.Electric.EnergyId;
        var testCases = new[]
        {
            new { EnergyId = -1, Quantity = 1, ExpectedError = "Invalid energy ID" },
            new { EnergyId = validId, Quantity = 0, ExpectedError = "Quantity must be positive" },
            new { EnergyId = validId, Quantity = -5, ExpectedError = "Invalid quantity" },
            new { EnergyId = validId, Quantity = stock.Electric.QuantityAvailable + 1, ExpectedError = "Insufficient stock" }
        };

        foreach (var testCase in testCases)
        {
            // Act
            var ex = Assert.Throws<ApplicationException>(() => 
                _energyService.BuyEnergy(testCase.EnergyId, testCase.Quantity));
            
            // Assert
            ex.Should().NotBeNull();
            ex.Message.Should().ContainAny("400", "401", "Bad Request");
            
            // Parse the error response from the exception message
            var errorResponse = TryParseErrorResponse(ex.Message);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().BeGreaterThanOrEqualTo(400).And.BeLessThan(500);
            errorResponse.Message.Should().ContainAny(
                testCase.ExpectedError.Split(' ') // Check for keywords in error message
            );
        }
    }

    private ErrorResponse TryParseErrorResponse(string message)
    {
        try
        {
            // Extract JSON substring from exception message
            var jsonStart = message.IndexOf('{');
            var jsonEnd = message.LastIndexOf('}') + 1;
            if (jsonStart >= 0 && jsonEnd > jsonStart)
            {
                var json = message.Substring(jsonStart, jsonEnd - jsonStart);
                return JsonConvert.DeserializeObject<ErrorResponse>(json);
            }
        }
        catch
        {
            // Fallback if parsing fails
        }
        return new ErrorResponse { Message = message };
    }

    [Test]
    public void GetEnergyStock_ShouldReturnValidData()
    {
        // Act
        var stock = _energyService.GetEnergyStock();

        // Assert
        stock.Should().NotBeNull();
        
        // Validate electric
        stock.Electric.EnergyId.Should().BeGreaterThan(0);
        stock.Electric.PricePerUnit.Should().BeGreaterThan(0);
        stock.Electric.QuantityAvailable.Should().BeGreaterThanOrEqualTo(0);
        stock.Electric.EnergyType.Should().Be("electric");
        stock.Electric.SellerName.Should().NotBeNullOrWhiteSpace();
        
        // Validate gas
        stock.Gas.EnergyId.Should().BeGreaterThan(0);
        stock.Gas.PricePerUnit.Should().BeGreaterThan(0);
        stock.Gas.QuantityAvailable.Should().BeGreaterThanOrEqualTo(0);
        stock.Gas.EnergyType.Should().Be("gas");
        
        // Validate oil
        stock.Oil.EnergyId.Should().BeGreaterThan(0);
        stock.Oil.PricePerUnit.Should().BeGreaterThan(0);
        stock.Oil.QuantityAvailable.Should().BeGreaterThanOrEqualTo(0);
        stock.Oil.EnergyType.Should().Be("oil");
    }

    [Test]
    public void DeleteOrder_ShouldRemoveOrderFromSystem()
    {
        // Arrange
        var oilId = _energyService.GetEnergyStock().Oil.EnergyId;
        var order = _energyService.BuyEnergy(oilId, 3);
        var initialCount = _energyService.GetAllOrders().Orders.Count;

        // Act
        var deleteSuccess = _energyService.DeleteOrder(order.OrderId);

        // Assert
        deleteSuccess.Should().BeTrue();
        _energyService.GetAllOrders().Orders.Count.Should().Be(initialCount - 1);
        
        // Verify order no longer exists
        var ex = Assert.Throws<ApplicationException>(() => 
            _energyService.GetOrderById(order.OrderId));
        ex.Message.Should().Contain("404");
    }

    [Test]
    public void BuyEnergy_WithInvalidToken_ShouldReturnUnauthorized()
    {
        // Arrange
        var invalidTokenService = new EnergyService(_apiClient, "invalid_token");
        var electricId = _energyService.GetEnergyStock().Electric.EnergyId;

        // Act & Assert
        var ex = Assert.Throws<ApplicationException>(() => 
            invalidTokenService.BuyEnergy(electricId, 1));
        
        ex.Message.Should().Contain("401");
    }
}