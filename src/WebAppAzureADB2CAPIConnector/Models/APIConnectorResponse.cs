using System.Text.Json.Serialization;

namespace WebApp.Models;

public class APIConnectorResponse
{
    [JsonPropertyName("version")]
    public string Version { get; set; }

    [JsonPropertyName("action")]
    public string Action { get; set; }

    [JsonPropertyName("userMessage")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string UserMessage { get; set; }
}
