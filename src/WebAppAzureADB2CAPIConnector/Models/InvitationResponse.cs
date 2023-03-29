using System.Text.Json.Serialization;

namespace WebApp.Models;

public class InvitationResponse
{
    [JsonPropertyName("invitations")]
    public List<string> Invitations { get; set; }
}
