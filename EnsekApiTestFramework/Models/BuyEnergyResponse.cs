using Newtonsoft.Json;

namespace EnsekApiTest.Models
{
    public class BuyEnergyResponse
    {
        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        [JsonProperty("energyType")]
        public string EnergyType { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("timeCreated")]
        public string TimeCreated { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        public DateTime GetOrderDateTime()
        {
            return DateTime.Parse(TimeCreated);
        }

        public bool IsCompleted()
        {
            return Status.Equals("COMPLETED", StringComparison.OrdinalIgnoreCase);
        }
    }
}