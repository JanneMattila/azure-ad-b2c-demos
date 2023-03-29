using System.Text.Json.Serialization;
using WebAppAzureADB2CAPIConnector.Models;

namespace WebApp.Models;

public class APIConnectorRequest
{
    [JsonPropertyName("step")]
    public string Step { get; set; }

    [JsonPropertyName("objectId")]
    public string ObjectID { get; set; }

    [JsonPropertyName("givenName")]
    public string GivenName { get; set; }

    [JsonPropertyName("surname")]
    public string Surname { get; set; }

    [JsonPropertyName("jobTitle")]
    public string JobTitle { get; set; }

    [JsonPropertyName("streetAddress")]
    public string StreetAddress { get; set; }

    [JsonPropertyName("city")]
    public string City { get; set; }

    [JsonPropertyName("postalCode")]
    public string PostalCode { get; set; }

    [JsonPropertyName("state")]
    public string State { get; set; }

    [JsonPropertyName("country")]
    public string Country { get; set; }

    [JsonPropertyName("client_id")]
    public string ClientID { get; set; }

    [JsonPropertyName("ui_locales")]
    public string UiLocales { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("displayName")]
    public string DisplayName { get; set; }

    [JsonPropertyName("identities")]
    public List<APIConnectorIdentity> Identities { get; set; }
}
