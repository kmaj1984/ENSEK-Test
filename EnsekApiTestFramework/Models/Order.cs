using Newtonsoft.Json;

namespace EnsekApiTestFramework.Models
{
    public class Order
    {
        [JsonProperty("id")]
        public string Id { get; set; }

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

        public bool IsBeforeDate(DateTime date)
        {
            return GetOrderDateTime() < date;
        }
    }
}