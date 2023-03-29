using System.Text.Json.Serialization;

namespace WebAppAzureADB2CAPIConnector.Models;

public class APIConnectorIdentity
{
    [JsonPropertyName("signInType")]
    public string SignInType { get; set; }

    [JsonPropertyName("issuer")]
    public string Issuer { get; set; }

    [JsonPropertyName("issuerAssignedId")]
    public string IssuerAssignedId { get; set; }
}
