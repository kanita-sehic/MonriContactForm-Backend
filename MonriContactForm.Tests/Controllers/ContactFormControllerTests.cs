using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MonriContactForm.App.Controllers;
using MonriContactForm.Core.Interfaces.Services;
using MonriContactForm.Core.Models.Requests;
using Moq;

namespace MonriContactForm.Tests.Controllers;

public class ContactFormControllerTests
{
    private readonly Mock<IContactFormService> _mockContactFormService = new();
    private readonly Mock<ILogger<ContactFormController>> _mockLogger = new();
    private readonly ContactFormController _controller;

    public ContactFormControllerTests()
    {
        _controller = new ContactFormController(_mockContactFormService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task ContactUser_ReturnsOk()
    {
        // Arrange
        var request = new ContactUserRequest
        {
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane@example.com"
        };

        _mockContactFormService
            .Setup(s => s.ContactUserAsync(request))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.ContactUser(request);

        // Assert
        var okResult = Assert.IsType<OkResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task ContactUser_LogsInformation()
    {
        // Arrange
        var request = new ContactUserRequest
        {
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane@example.com"
        };

        _mockContactFormService
            .Setup(s => s.ContactUserAsync(It.IsAny<ContactUserRequest>()))
            .Returns(Task.CompletedTask);

        // Act
        await _controller.ContactUser(request);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Contact user called with request")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}
