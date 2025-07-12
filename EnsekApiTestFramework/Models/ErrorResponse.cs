using Newtonsoft.Json;

namespace EnsekApiTest.Models
{
    public class ErrorResponse
    {
        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        public override string ToString()
        {
            return $"API Error: {Error} | Message: {Message} | Time: {Timestamp}";
        }
    }
}