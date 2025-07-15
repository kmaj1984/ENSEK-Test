using RestSharp;

public class ApiClient
{
    private readonly RestClient _client;

    public ApiClient(string baseUrl)
    {
        _client = new RestClient(baseUrl);
        _client.AddDefaultHeader("Accept", "application/json");
    }

    public async Task<RestResponse> ExecuteAsync(RestRequest request)
    {
        return await _client.ExecuteAsync(request);
    }

    public RestResponse Execute(RestRequest request)
    {
        var response = _client.Execute(request);
        return response;
    }
}