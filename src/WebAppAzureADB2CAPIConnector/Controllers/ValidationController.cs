using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers;

[Produces("application/json")]
[ApiController]
[Route("api/[controller]")]
public class ValidationController : ControllerBase
{
    private readonly ILogger<ValidationController> _logger;

    public ValidationController(ILogger<ValidationController> logger)
    {
        _logger = logger;
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
    public ObjectResult Post(APIConnectorRequest request)
    {
        var response = new APIConnectorResponse()
        {
            Version = "1.0.0",
            Action = "ShowBlockPage",
            UserMessage = "You are not able to sign up at this time. Please reach out to your support contact."
        };

        return new ObjectResult(response);
    }
}
