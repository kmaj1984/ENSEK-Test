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

        if (!response.IsSuccessful)
        {
            throw new ApplicationException(
                $"Reset failed: {response.StatusCode}. " +
                $"Error: {response.ErrorMessage ?? JsonConvert.DeserializeObject<ErrorResponse>(response.Content)?.Message}"
            );
        }

        return JsonConvert.DeserializeObject<ResetResponse>(response.Content);
    }

    public BuyEnergyResponse BuyEnergy(int energyId, int quantity)
    {
        var request = new RestRequest($"ENSEK/buy/{energyId}/{quantity}", Method.Put);
        request.AddHeader("accept", "application/json");
        request.AddHeader("Authorization", $"Authorization");
        var response = _apiClient.Execute(request);

        if (!response.IsSuccessful)
        {
            var error = JsonConvert.DeserializeObject<ErrorResponse>(response.Content);
            throw new ApplicationException(
                $"Energy purchase failed ({response.StatusCode}): {error?.Message ?? response.ErrorMessage}"
            );
        }

        return JsonConvert.DeserializeObject<BuyEnergyResponse>(response.Content);
    }

    public OrderListResponse GetAllOrders()
    {
        var request = new RestRequest("ENSEK/orders", Method.Get);
        request.AddHeader("accept", "application/json");
        request.AddHeader("Authorization", $"Authorization");
        var response = _apiClient.Execute(request);

        if (!response.IsSuccessful)
        {
            throw new ApplicationException(
                $"Failed to get orders: {response.StatusCode}. " +
                $"Error: {response.ErrorMessage ?? JsonConvert.DeserializeObject<ErrorResponse>(response.Content)?.Message}"
            );
        }

        return JsonConvert.DeserializeObject<OrderListResponse>(response.Content);
    }

    public EnergyStockResponse GetEnergyStock()
    {
        var request = new RestRequest("ENSEK/energy", Method.Get);
        request.AddHeader("accept", "application/json");
        request.AddHeader("Authorization", $"Authorization");
        var response = _apiClient.Execute(request);

        if (!response.IsSuccessful)
        {
            throw new ApplicationException(
                $"Failed to get energy stock: {response.StatusCode}. " +
                $"Error: {response.ErrorMessage ?? JsonConvert.DeserializeObject<ErrorResponse>(response.Content)?.Message}"
            );
        }

        return JsonConvert.DeserializeObject<EnergyStockResponse>(response.Content);
    }

    public Order GetOrderById(string orderId)
    {
        var request = new RestRequest($"ENSEK/orders/{orderId}", Method.Get);
        request.AddHeader("accept", "application/json");
        request.AddHeader("Authorization", $"Authorization");
        var response = _apiClient.Execute(request);

        if (!response.IsSuccessful)
        {
            throw new ApplicationException(
                $"Failed to get order {orderId}: {response.StatusCode}. " +
                $"Error: {response.ErrorMessage ?? JsonConvert.DeserializeObject<ErrorResponse>(response.Content)?.Message}"
            );
        }

        return JsonConvert.DeserializeObject<Order>(response.Content);
    }

    public bool DeleteOrder(string orderId)
    {
        var request = new RestRequest($"ENSEK/orders/{orderId}", Method.Delete);
        request.AddHeader("Authorization", $"Bearer {_authToken}");
        var response = _apiClient.Execute(request);
        return response.IsSuccessful;
    }
}