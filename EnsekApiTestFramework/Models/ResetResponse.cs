using Newtonsoft.Json;

public class ResetResponse
{
    [JsonProperty("message")]
    public string Message { get; set; }

    [JsonProperty("timeStamp")]
    public string TimeStamp { get; set; }

    public bool IsSuccess()
    {
        return Message?.Contains("successfully", StringComparison.OrdinalIgnoreCase) ?? false;
    }

    public DateTime GetResetDateTime()
    {
        return DateTime.Parse(TimeStamp);
    }
}