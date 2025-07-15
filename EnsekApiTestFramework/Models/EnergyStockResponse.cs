using Newtonsoft.Json;

namespace EnsekApiTestFramework.Models;

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