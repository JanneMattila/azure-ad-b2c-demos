using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System.Text;
using WebApp.Controllers;
using WebApp.Models;
using WebAppAzureADB2CAPIConnector.Data;
using WebAppAzureADB2CAPIConnector.Models;

namespace WebAppAzureADB2CAPIConnectorTests;

public class ValidationControllerTests
{
    private readonly InvitationRepository _invitationRepository;
    private readonly ValidationController _validationController;
    private readonly ValidationOptions _validationOptions;

    public ValidationControllerTests()
    {
        _validationOptions = new ValidationOptions();
        _invitationRepository = new InvitationRepository();

        var nullLoggerFactory = new NullLoggerFactory();
        _validationController = new ValidationController(nullLoggerFactory.CreateLogger<ValidationController>(), _invitationRepository, Options.Create(_validationOptions));
        _validationController.ControllerContext.HttpContext = new DefaultHttpContext();
    }

    [Fact]
    public void Unauthorized_Test()
    {
        // Arrange & Act
        var actual = _validationController.Post(null);

        // Assert
        Assert.IsType<UnauthorizedResult>(actual);
    }

    [Fact]
    public void Authenticate_Test()
    {
        // Arrange
        var base64value = Convert.ToBase64String(Encoding.UTF8.GetBytes("user:password"));
        _validationController.ControllerContext.HttpContext.Request.Headers.Add("Authorization", $"Basic {base64value}");
        _validationOptions.Username = "user";
        _validationOptions.Password = "password";
        var expected = true;


        // Act
        var actual = _validationController.Authenticate();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void User_Not_In_Invitation_List_Test()
    {
        // Arrange
        _validationOptions.SkipBasicAuthentication = true;
        var request = new APIConnectorRequest() { Email = "user@contoso.com" };
        var expectedAction = "ShowBlockPage";

        // Act
        var actual = _validationController.Post(request);

        // Assert
        Assert.IsType<OkObjectResult>(actual);
        var okObjectResult = actual as OkObjectResult;
        Assert.IsType<APIConnectorResponse>(okObjectResult?.Value);
        var response = okObjectResult.Value as APIConnectorResponse;
        Assert.Equal(expectedAction, response?.Action);
    }

    [Fact]
    public void User_In_Invitation_List_Test()
    {
        // Arrange
        _validationOptions.SkipBasicAuthentication = true;
        _invitationRepository.Add("user@contoso.com", string.Empty);
        var request = new APIConnectorRequest() { Email = "user@contoso.com" };
        var expectedAction = "Continue";
        var expectedRepositoryUserCount = 0;

        // Act
        var actual = _validationController.Post(request);

        // Assert
        Assert.IsType<OkObjectResult>(actual);
        var okObjectResult = actual as OkObjectResult;
        Assert.IsType<APIConnectorResponse>(okObjectResult?.Value);
        var response = okObjectResult.Value as APIConnectorResponse;
        Assert.Equal(expectedAction, response?.Action);
        Assert.Equal(expectedRepositoryUserCount, _invitationRepository.GetAllInvitations().Count);
    }

    [Fact]
    public void Require_Invitation_Code_User_Not_In_Invitation_List_Test()
    {
        // Arrange
        _validationOptions.SkipBasicAuthentication = true;
        _validationOptions.InvitationCodeAttributeField = "extension_af3257c73cb0499da4b1ef4e0db3be1f_InvitationCode";
        var request = new APIConnectorRequest() { Email = "user@contoso.com" };
        var expectedAction = "ValidationError";
        var expectedStatus = "400";

        // Act
        var actual = _validationController.Post(request);

        // Assert
        Assert.IsType<BadRequestObjectResult>(actual);
        var badRequestObjectResult = actual as BadRequestObjectResult;
        Assert.IsType<APIConnectorResponse>(badRequestObjectResult?.Value);
        var response = badRequestObjectResult.Value as APIConnectorResponse;
        Assert.Equal(expectedAction, response?.Action);
        Assert.Equal(expectedStatus, response?.Status);
    }

    [Fact]
    public void Require_Invitation_Code_User_In_Invitation_List_Test()
    {
        // Arrange
        _validationOptions.SkipBasicAuthentication = true;
        _invitationRepository.Add("user@contoso.com", "12345");
        _validationOptions.InvitationCodeAttributeField = "extension_af3257c73cb0499da4b1ef4e0db3be1f_InvitationCode";
        var request = new APIConnectorRequest
        {
            Email = "user@contoso.com",
            AdditionalFields = new Dictionary<string, object>()
            {
                { "extension_af3257c73cb0499da4b1ef4e0db3be1f_InvitationCode", "12345" }
            }
        };
        var expectedAction = "Continue";
        var expectedInvitationCodeField = "";
        var expectedRepositoryUserCount = 0;

        // Act
        var actual = _validationController.Post(request);

        // Assert
        Assert.IsType<OkObjectResult>(actual);
        var okObjectResult = actual as OkObjectResult;
        Assert.IsType<Dictionary<string, string>>(okObjectResult?.Value);
        var response = okObjectResult.Value as Dictionary<string, string>;
        Assert.Equal(expectedAction, response?["action"]);
        Assert.Equal(expectedInvitationCodeField, response?["extension_af3257c73cb0499da4b1ef4e0db3be1f_InvitationCode"]);
        Assert.Equal(expectedRepositoryUserCount, _invitationRepository.GetAllInvitations().Count);
    }
}
