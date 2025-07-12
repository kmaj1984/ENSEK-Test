using RestSharp;

public class ApiClient
{
    private readonly RestClient _client;
    private readonly string _baseUrl;

    public ApiClient(string baseUrl)
    {
        _baseUrl = baseUrl;
        _client = new RestClient(baseUrl);
    }

    public async Task<RestResponse> ExecuteAsync(RestRequest request)
    {
        return await _client.ExecuteAsync(request);
    }

    public RestResponse Execute(RestRequest request)
    {
        return _client.Execute(request);
    }
}