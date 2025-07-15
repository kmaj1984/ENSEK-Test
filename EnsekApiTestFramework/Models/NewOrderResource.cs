using Newtonsoft.Json;

namespace EnsekApiTestFramework.Models
{
    public class OrderResource
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("energy_id")]  // Matches Swagger definition
        public int EnergyId { get; set; }
    }
}