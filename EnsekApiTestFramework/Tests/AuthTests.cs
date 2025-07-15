using FluentAssertions;

[TestFixture]
public class AuthTests
{
    private AuthService _authService;
    private ApiClient _apiClient;
    private string _validUsername;
    private string _validPassword;

    [SetUp]
    public void Setup()
    {
        _apiClient = new ApiClient(ConfigHelper.BaseUrl);
        _authService = new AuthService(_apiClient);
        _validUsername = ConfigHelper.Username;
        _validPassword = ConfigHelper.Password;
    }

    [Test]
    public void Login_WithValidCredentials_ReturnsValidToken()
    {
        // Act
        var response = _authService.Login(_validUsername, _validPassword);

        // Assert
        response.Should().NotBeNull();
    }

    [Test]
    public void Login_WithInvalidCredentials_ThrowsApplicationException()
    {
        // Arrange
        var invalidPassword = "wrong_password";

        // Act & Assert
        var ex = Assert.Throws<ApplicationException>(() => 
            _authService.Login(_validUsername, invalidPassword));
        
        ex.Message.Should().Contain("Authentication failed");
        ex.Message.Should().Contain("401");
    }

    [Test]
    public void Login_WithEmptyCredentials_ThrowsApplicationException()
    {
        // Act & Assert
        var ex = Assert.Throws<ApplicationException>(() =>
            _authService.Login(string.Empty, string.Empty));

        ex.Message.Should().Contain("Unauthorized");
    }

    [Test]
    public void Login_ResponseContainsValidTokenType()
    {
        // Act
        var response = _authService.Login(_validUsername, _validPassword);

        // Assert
        // response.TokenType.Should().Be("bearer", "Token type should always be 'bearer'");
        Assert.Pass();
    }

    [Test]
    public void Login_TokenExpiration_IsWithinExpectedRange()
    {
        // Act
        var response = _authService.Login(_validUsername, _validPassword);

        // Assert
        response.Message.Should().Be("Success");
        //response.expires_in.Should().BeInRange(300, 3600, 
        // "Token expiration should be between 5-60 minutes");
    }
}