using System.Text.Json.Serialization;

namespace WebApp.Models;

public class Invitation
{
    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("invitationCode")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string InvitationCode { get; set; }
}
