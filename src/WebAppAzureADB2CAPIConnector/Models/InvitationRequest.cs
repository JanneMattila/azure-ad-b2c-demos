using System.Text.Json.Serialization;

namespace WebApp.Models;

public class InvitationRequest
{
    [JsonPropertyName("email")]
    public string Email { get; set; }
}
