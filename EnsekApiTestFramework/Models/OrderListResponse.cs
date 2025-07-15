using Newtonsoft.Json;
using System.Collections.Generic;

namespace EnsekApiTestFramework.Models
{
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
}