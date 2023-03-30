namespace WebAppAzureADB2CAPIConnector.Models;

public class ValidationOptions
{
    public bool SkipBasicAuthentication { get; set; }

    /// <summary>
    /// Plaintext username
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Base64 encoded password
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Extension attribute name for invitation code
    /// Format:
    /// "extension_{B2C_EXTENSIONS_APP_ID}_InvitationCode"
    /// Example:
    /// "extension_af3257c7-3cb0-499d-a4b1-ef4e0db3be1f_InvitationCode"
    /// </summary>
    public string InvitationCodeAttributeField { get; set; }

    public string UserMessageBlocked { get; set; } = UserMessageConstants.Blocked;
    public string UserMessageInvitationCodeMissing { get; set; } = UserMessageConstants.InvitationCodeMissing;
    public string UserMessageInvalidInvitationCode { get; set; } = UserMessageConstants.InvalidInvitationCode;
}
