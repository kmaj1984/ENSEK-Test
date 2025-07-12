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
    var request = new RestRequest("api/auth", Method.Post); // Changed from "ENSEK/auth"
    request.AddJsonBody(new { username, password });
    
    var response = _apiClient.Execute(request);
    
    if (!response.IsSuccessful)
    {
        throw new ApplicationException(
            $"Authentication failed: {response.StatusCode}. " +
            $"Response: {response.Content}"
        );
    }

    return JsonConvert.DeserializeObject<LoginResponse>(response.Content);
}

    public bool ValidateToken(string token)
    {
        var request = new RestRequest("ENSEK/validate", Method.Get);
        request.AddHeader("Authorization", $"Bearer {token}");
        var response = _apiClient.Execute(request);
        return response.IsSuccessful;
    }

    public bool Logout(string token)
    {
        var request = new RestRequest("ENSEK/logout", Method.Post);
        request.AddHeader("Authorization", $"Bearer {token}");
        var response = _apiClient.Execute(request);
        return response.IsSuccessful;
    }

    public LoginResponse RefreshToken(string refreshToken)
    {
        var request = new RestRequest("ENSEK/refresh", Method.Post);
        request.AddHeader("Authorization", $"Bearer {refreshToken}");
        var response = _apiClient.Execute(request);
        return JsonConvert.DeserializeObject<LoginResponse>(response.Content);
    }
}