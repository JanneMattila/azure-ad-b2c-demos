using System.Text.Json.Serialization;

namespace WebApp.Models;

public class AddInvitationResponse
{
    [JsonPropertyName("server")]
    public string Server { get; set; }

    [JsonPropertyName("path")]
    public string Path { get; set; }

    [JsonPropertyName("filesCreated")]
    public int FilesCreated { get; set; }

    [JsonPropertyName("milliseconds")]
    public double Milliseconds { get; set; }
}
