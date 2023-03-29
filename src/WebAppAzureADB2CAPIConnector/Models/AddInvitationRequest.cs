using System.Text.Json.Serialization;

namespace WebApp.Models;

public class AddInvitationRequest
{
    [JsonPropertyName("path")]
    public string Path { get; set; }

    [JsonPropertyName("folders")]
    public int Folders { get; set; }

    [JsonPropertyName("subFolders")]
    public int SubFolders { get; set; }

    [JsonPropertyName("filesPerFolder")]
    public int FilesPerFolder { get; set; }

    [JsonPropertyName("fileSize")]
    public int FileSize { get; set; }
}
