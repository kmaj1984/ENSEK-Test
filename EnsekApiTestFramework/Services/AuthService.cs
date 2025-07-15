using RestSharp;
using Newtonsoft.Json;

public class AuthService
{
    private readonly ApiClient _apiClient;

    public AuthService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public LoginResponse Login(string username, string password)
    {
        var request = new RestRequest("ENSEK/login", Method.Post);
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Accept", "application/json");

        // Add this only if you have a real token
        request.AddHeader("Authorization", "Bearer your-token-here");

        request.AddJsonBody(new LoginRequest
        {
            Username = username,
            Password = password
        });

        var response = _apiClient.Execute(request);

        if (!response.IsSuccessful || string.IsNullOrEmpty(response.Content))
        {
            throw new ApplicationException(
                $"Authentication failed: {(int)response.StatusCode} {response.StatusDescription}. Response: {response.Content}"
            );
        }

        try
        {
            return JsonConvert.DeserializeObject<LoginResponse>(response.Content);
        }
        catch (JsonException ex)
        {
            throw new ApplicationException("Failed to parse authentication response.", ex);
        }
    }
}

public class LoginRequest
{
    [JsonProperty("username")]
    public string Username { get; set; }

    [JsonProperty("password")]
    public string Password { get; set; }
}

public class LoginResponse
{
    // Define properties based on the expected JSON response from the API.
    // Example:
     [JsonProperty("access_token")]
    public string AccessToken { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }

    // Add any other fields returned by the login endpoint.
}
