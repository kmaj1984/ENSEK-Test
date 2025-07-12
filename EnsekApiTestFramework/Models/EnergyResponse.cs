using Newtonsoft.Json;
using EnsekApiTestFramework.Models;

public class EnergyResponse
{
    [JsonProperty("energy_id")]
    public int EnergyId { get; set; }

    [JsonProperty("energy_type")]
    public string EnergyType { get; set; }

    [JsonProperty("price_per_unit")]
    public decimal PricePerUnit { get; set; }

    [JsonProperty("quantity_available")]
    public int QuantityAvailable { get; set; }

    [JsonProperty("seller_name")]
    public string SellerName { get; set; }
}

public class EnergyStockResponse
{
    [JsonProperty("electric")]
    public EnergyResponse Electric { get; set; }

    [JsonProperty("gas")]
    public EnergyResponse Gas { get; set; }

    [JsonProperty("oil")]
    public EnergyResponse Oil { get; set; }

    [JsonProperty("nuclear")]
    public EnergyResponse Nuclear { get; set; }
}

public class BuyEnergyResponse
{
    [JsonProperty("order_id")]
    public string OrderId { get; set; }

    [JsonProperty("energy_type")]
    public string EnergyType { get; set; }

    [JsonProperty("quantity")]
    public int Quantity { get; set; }

    [JsonProperty("total_cost")]
    public decimal TotalCost { get; set; }

    [JsonProperty("time_created")]
    public string TimeCreated { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }
}

public class OrderListResponse
{
    [JsonProperty("orders")]
    public List<Order> Orders { get; set; }

    [JsonProperty("total_orders")]
    public int TotalOrders { get; set; }

    [JsonProperty("page")]
    public int Page { get; set; }

    [JsonProperty("page_size")]
    public int PageSize { get; set; }
}

public class ErrorResponse
{
    [JsonProperty("error")]
    public string Error { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }

    [JsonProperty("status_code")]
    public int StatusCode { get; set; }

    [JsonProperty("timestamp")]
    public string Timestamp { get; set; }
}