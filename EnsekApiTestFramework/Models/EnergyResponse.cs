using Newtonsoft.Json;

namespace EnsekApiTestFramework.Models;

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