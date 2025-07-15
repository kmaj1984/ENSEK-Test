using Newtonsoft.Json;

namespace EnsekApiTestFramework.Models;
    public class ErrorResponse
    {
        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("status_code")]  // Added to match Swagger error patterns
        public int StatusCode { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        public override string ToString()
        {
            return $"API Error: {Error} | Message: {Message} | Status: {StatusCode} | Time: {Timestamp}";
        }
    }