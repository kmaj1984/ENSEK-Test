using Newtonsoft.Json;

namespace EnsekApiTestFramework.Models;
    public class BuyEnergyResponse
    {
        [JsonProperty("order_id")]  // Standardized to match Swagger
        public string OrderId { get; set; }

        [JsonProperty("energy_id")]  // Added to match Swagger
        public int EnergyId { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("total_cost")]  // Added from other definition
        public decimal TotalCost { get; set; }

        [JsonProperty("time_created")]
        public string TimeCreated { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        public DateTime? GetOrderDateTime()
        {
            return !string.IsNullOrEmpty(TimeCreated) 
                ? DateTime.Parse(TimeCreated) 
                : null;
        }

        public bool IsCompleted()
        {
            return Status?.Equals("COMPLETED", StringComparison.OrdinalIgnoreCase) ?? false;
        }
    }