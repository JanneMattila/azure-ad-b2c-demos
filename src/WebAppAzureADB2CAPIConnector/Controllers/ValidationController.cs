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

        // Are we using invitation code validation?
        if (string.IsNullOrEmpty(_options.InvitationCodeAttributeField))
        {
            // No invitation code attribute implemented, so skip validating invitation code
            if (_invitationRepository.Contains(request.Email))
            {
                // This user is in our invitation list
                _invitationRepository.Remove(request.Email);
                return Ok(new APIConnectorResponse()
                {
                    Version = version,
                    Action = "Continue"
                });
            }

            // User is not in the invitation list. Show block page.
            return Ok(new APIConnectorResponse()
            {
                Version = version,
                Action = "ShowBlockPage",
                UserMessage = _options.UserMessageBlocked
            });
        }
        else
        {
            // Invitation code verification is in use
            if (_invitationRepository.Contains(request.Email) &&
                request.AdditionalFields.ContainsKey(_options.InvitationCodeAttributeField))
            {
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
                    return Ok(response);
                }
            }

            // If we couldn't succesfully verify code, then we'll force user to re-input the code.
            return BadRequest(new APIConnectorResponse()
            {
                Version = version,
                Action = "ValidationError",
                UserMessage = _options.UserMessageValidInvitationCodeRequired
            });
        }
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