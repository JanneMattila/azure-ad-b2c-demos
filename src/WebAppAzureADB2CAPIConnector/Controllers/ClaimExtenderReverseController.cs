using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using System.Text;
using WebApp.Models;
using WebAppAzureADB2CAPIConnector.Data;
using WebAppAzureADB2CAPIConnector.Models;

namespace WebApp.Controllers;

[Produces("application/json")]
[ApiController]
[Route("api/[controller]")]
public class ClaimExtenderReverseController : ControllerBase
{
    private readonly ILogger<ClaimExtenderReverseController> _logger;

    public ClaimExtenderReverseController(ILogger<ClaimExtenderReverseController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Return additional claims to user.
    /// </summary>
    /// <param name="request">Incoming claims</param>
    /// <returns>API Connector response</returns>
    /// <response code="200">Returns output claims</response>
    /// <response code="400">If request parameters are incorrectly defined</response>
    /// <response code="500">If errors occur</response>
    [HttpPost]
    [ProducesResponseType(typeof(Dictionary<string, object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult Post(Dictionary<string, object> body)
    {
        var reverseOption = body.TryGetValue("option", out object value) ? new string(value.ToString().Reverse().ToArray()) : null;
        return Ok(new
        {
            TextReversed = reverseOption
        });
    }
}
