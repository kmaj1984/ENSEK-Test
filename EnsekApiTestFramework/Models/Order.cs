using Newtonsoft.Json;

namespace EnsekApiTestFramework.Models
{
    public class Order
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        // Changed to match Swagger's energy_id
        [JsonProperty("energy_id")]
        public int EnergyId { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("time_created")]  // Standardized naming
        public string TimeCreated { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        public DateTime GetOrderDateTime()
        {
            return DateTime.Parse(TimeCreated);
        }

        public bool IsBeforeDate(DateTime date)
        {
            return GetOrderDateTime() < date;
        }
    }
}