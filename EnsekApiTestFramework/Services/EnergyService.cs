using RestSharp;
using Newtonsoft.Json;
using EnsekApiTestFramework.Models; 

public class EnergyService
{
    private readonly ApiClient _apiClient;
    private readonly string _authToken;

    public EnergyService(ApiClient apiClient, string authToken)
    {
        _apiClient = apiClient;
        _authToken = authToken;
    }

    public ResetResponse ResetTestData()
    {
        var request = new RestRequest("ENSEK/reset", Method.Post);
        request.AddHeader("Authorization", $"Bearer {_authToken}");
        var response = _apiClient.Execute(request);
        return JsonConvert.DeserializeObject<ResetResponse>(response.Content);
    }

    public BuyEnergyResponse BuyEnergy(string energyType, int quantity)
    {
        var request = new RestRequest($"ENSEK/buy/{energyType}/{quantity}", Method.Put);
        request.AddHeader("Authorization", $"Bearer {_authToken}");
        var response = _apiClient.Execute(request);
        
        if (!response.IsSuccessful)
        {
            throw new ApplicationException(
                $"Failed to buy energy: {response.StatusCode}. " +
                $"Error: {response.ErrorMessage ?? JsonConvert.DeserializeObject<ErrorResponse>(response.Content)?.Message}"
            );
        }

        return JsonConvert.DeserializeObject<BuyEnergyResponse>(response.Content);
    }

    public List<Order> GetAllOrders()
    {
        var request = new RestRequest("ENSEK/orders", Method.Get);
        request.AddHeader("Authorization", $"Bearer {_authToken}");
        var response = _apiClient.Execute(request);
        return JsonConvert.DeserializeObject<List<Order>>(response.Content);
    }

    public EnergyStockResponse GetEnergyStock()
    {
        var request = new RestRequest("ENSEK/energy", Method.Get);
        request.AddHeader("Authorization", $"Bearer {_authToken}");
        var response = _apiClient.Execute(request);
        return JsonConvert.DeserializeObject<EnergyStockResponse>(response.Content);
    }
}