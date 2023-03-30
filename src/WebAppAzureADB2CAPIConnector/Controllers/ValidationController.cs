using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using WebApp.Models;
using WebAppAzureADB2CAPIConnector.Data;
using WebAppAzureADB2CAPIConnector.Models;

namespace WebApp.Controllers;

/// <summary>
/// See also example:
/// https://github.com/Azure-Samples/active-directory-b2c-node-sign-up-user-flow-invitation-code/blob/main/InvitationCodeAzureFunction/index.js
/// </summary>
[Produces("application/json")]
[ApiController]
[Route("api/[controller]")]
public class ValidationController : ControllerBase
{
    private readonly ILogger<ValidationController> _logger;
    private readonly InvitationRepository _invitationRepository;
    private readonly ValidationOptions _options;

    public ValidationController(ILogger<ValidationController> logger, InvitationRepository invitationRepository, IOptions<ValidationOptions> options)
    {
        _logger = logger;
        _invitationRepository = invitationRepository;
        _options = options.Value;
    }

    /// <summary>
    /// Validates user sign-in request.
    /// </summary>
    /// <param name="request">API Connector request definition</param>
    /// <returns>API Connector response</returns>
    /// <response code="200">Returns files and folders found</response>
    /// <response code="400">If request parameters are incorrectly defined</response>
    /// <response code="500">If errors occur</response>
    [HttpPost]
    [ProducesResponseType(typeof(APIConnectorResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult Post(APIConnectorRequest request)
    {
        if (!Authenticate())
        {
            return Unauthorized();
        }

        var version = "1.0.0";

        if (!_invitationRepository.Contains(request.Email))
        {
            var blockedResponse = new APIConnectorResponse()
            {
                Version = version,
                Action = "ShowBlockPage",
                UserMessage = _options.UserMessageBlocked
            };
            return BadRequest(blockedResponse);
        }

        if (string.IsNullOrEmpty(_options.InvitationCodeAttributeField))
        {
            // No invitation extension implemented, so skip validating it.
            var response = new APIConnectorResponse()
            {
                Version = version,
                Action = "Continue"
            };
            _invitationRepository.Remove(request.Email);
            return new ObjectResult(response);
        }

        // Validate incoming invitation code
        if (!request.AdditionalFields.ContainsKey(_options.InvitationCodeAttributeField))
        {
            var invitationCodeMissingResponse = new APIConnectorResponse()
            {
                Version = version,
                Action = "ValidationError",
                UserMessage = _options.UserMessageInvitationCodeMissing
            };
            return BadRequest(invitationCodeMissingResponse);
        }

        var storedInvitationCode = _invitationRepository.Get(request.Email);
        var incomingInvitationCode = request.AdditionalFields[_options.InvitationCodeAttributeField].ToString();

        if (storedInvitationCode == incomingInvitationCode)
        {
            // Invitation code validated successfully
            _invitationRepository.Remove(request.Email);
            var response = new Dictionary<string, string>()
            {
                { "version", version },
                { "action", "Continue" },
                { _options.InvitationCodeAttributeField, "" } // Clean up invitation field data from directory
            };
            return new OkObjectResult(response);
        }

        var invalidInvitationCodeResponse = new APIConnectorResponse()
        {
            Version = version,
            Action = "ShowBlockPage",
            UserMessage = _options.UserMessageInvalidInvitationCode
        };
        return BadRequest(invalidInvitationCodeResponse);
    }

    private bool Authenticate()
    {
        if (_options.SkipBasicAuthentication)
        {
            // Skip basic authentication
            return true;
        }

        if (Request.Headers.TryGetValue(HeaderNames.Authorization, out var value))
        {
            if (AuthenticationHeaderValue.TryParse(value.First(), out var parsedValue))
            {
                if (string.Compare(parsedValue.Scheme, "basic", true) == 0)
                {
                    var parts = parsedValue.Parameter.Split(':');
                    if (parts.Length == 2)
                    {
                        if (parts[0] == _options.Username &&
                            parts[1] == _options.Password)
                        {
                            return true;
                        }
                    }
                }
            }
        }

        // Authentication failed
        return false;
    }
}