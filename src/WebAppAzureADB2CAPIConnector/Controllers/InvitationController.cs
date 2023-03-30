using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebAppAzureADB2CAPIConnector.Data;

namespace WebApp.Controllers;

[Produces("application/json")]
[ApiController]
[Route("api/[controller]")]
public class InvitationController : ControllerBase
{
    private readonly ILogger<InvitationController> _logger;
    private readonly InvitationRepository _invitationRepository;

    public InvitationController(ILogger<InvitationController> logger, InvitationRepository invitationRepository)
    {
        _logger = logger;
        _invitationRepository = invitationRepository;
    }

    /// <summary>
    /// Get all invitations.
    /// </summary>
    /// <response code="200">All available invitations</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public OkObjectResult Get()
    {
        var invitations = _invitationRepository.GetAllInvitations();
        return Ok(invitations);
    }

    /// <summary>
    /// Add invitation request.
    /// </summary>
    /// <param name="request">Add invitation request definition</param>
    /// <response code="200">Invitation added</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public OkResult Post(InvitationRequest request)
    {
        _invitationRepository.Add(request.Email, request.InvitationCode);
        return Ok();
    }

    /// <summary>
    /// Delete invitation request.
    /// </summary>
    /// <param name="request">Delete invitation request definition</param>
    /// <response code="200">Invitation deleted</response>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public OkResult Delete(InvitationRequest request)
    {
        _invitationRepository.Remove(request.Email);
        return Ok();
    }
}
