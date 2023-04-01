using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System.Text;
using WebApp.Controllers;
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
        var excepted = true;
        var base64value = Convert.ToBase64String(Encoding.UTF8.GetBytes("user:password"));
        _validationController.ControllerContext.HttpContext.Request.Headers.Add("Authorization", $"Basic {base64value}");
        _validationOptions.Username = "user";
        _validationOptions.Password = "password";


        // Act
        var actual = _validationController.Authenticate();

        // Assert
        Assert.Equal(excepted, actual);
    }
}
