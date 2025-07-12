using FluentAssertions;
using NUnit.Framework;
using System;
using System.Net;

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
        var config = ConfigHelper.GetConfiguration();
        _apiClient = new ApiClient(config["ApiSettings:BaseUrl"]);
        _authService = new AuthService(_apiClient);
        _validUsername = config["ApiSettings:Username"];
        _validPassword = config["ApiSettings:Password"];
    }

    [Test]
    public void Login_WithValidCredentials_ReturnsValidToken()
    {
        // Act
        var response = _authService.Login(_validUsername, _validPassword);

        // Assert
        response.Should().NotBeNull();
        response.access_token.Should().NotBeNullOrWhiteSpace();
        response.token_type.Should().Be("bearer");
        response.expires_in.Should().BeGreaterThan(0);
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
        ex.Message.Should().Contain("Unauthorized");
    }

    [Test]
    public void ValidateToken_WithValidToken_ReturnsTrue()
    {
        // Arrange
        var loginResponse = _authService.Login(_validUsername, _validPassword);

        // Act
        var isValid = _authService.ValidateToken(loginResponse.access_token);

        // Assert
        isValid.Should().BeTrue();
    }

    [Test]
    public void ValidateToken_WithInvalidToken_ReturnsFalse()
    {
        // Act
        var isValid = _authService.ValidateToken("invalid_token");

        // Assert
        isValid.Should().BeFalse();
    }

    [Test]
    public void RefreshToken_WithValidRefreshToken_ReturnsNewToken()
    {
        // Arrange
        var loginResponse = _authService.Login(_validUsername, _validPassword);

        // Act
        var refreshResponse = _authService.RefreshToken(loginResponse.refresh_token);

        // Assert
        refreshResponse.Should().NotBeNull();
        refreshResponse.access_token.Should().NotBeNullOrWhiteSpace();
        refreshResponse.access_token.Should().NotBe(loginResponse.access_token);
    }

    [Test]
    public void Logout_WithValidToken_InvalidatesToken()
    {
        // Arrange
        var loginResponse = _authService.Login(_validUsername, _validPassword);
        var initialValidation = _authService.ValidateToken(loginResponse.access_token);

        // Act
        var logoutResponse = _authService.Logout(loginResponse.access_token);
        var postLogoutValidation = _authService.ValidateToken(loginResponse.access_token);

        // Assert
        initialValidation.Should().BeTrue();
        logoutResponse.Should().BeTrue();
        postLogoutValidation.Should().BeFalse();
    }
}