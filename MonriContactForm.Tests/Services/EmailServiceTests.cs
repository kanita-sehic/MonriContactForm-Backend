using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MonriContactForm.Core.Configuration;
using MonriContactForm.Core.Exceptions;
using MonriContactForm.Infrastructure.Email.Services;
using Moq;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MonriContactForm.Tests.Services;

public class EmailServiceTests
{
    private readonly Mock<IOptions<AppSettings>> _optionsMock = new();
    private readonly Mock<ILogger<EmailService>> _loggerMock = new();
    private readonly Mock<ISendGridClient> _sendGridClientMock = new();
    private readonly EmailService _emailService;
    private readonly EmailConfiguration _emailConfig;

    public EmailServiceTests()
    {
        _emailConfig = new EmailConfiguration
        {
            ApiKey = "fake-api-key",
            FromEmail = "test@monri.com",
            FromName = "Monri"
        };

        var appSettings = new AppSettings
        {
            Email = new EmailConfiguration
            {
                ApiKey = "fake-api-key",
                FromEmail = "no-reply@example.com",
                FromName = "Test Sender"
            }
        };

        _optionsMock.Setup(o => o.Value).Returns(new AppSettings { Email = _emailConfig });
        _emailService = new EmailService(_optionsMock.Object, _loggerMock.Object, _sendGridClientMock.Object);
    }

    [Fact]
    public async Task SendEmailAsync_SuccessfulResponse_DoesNotThrow()
    {
        // Arrange
        var response = new Response(HttpStatusCode.Accepted, null, null);

        _sendGridClientMock
            .Setup(c => c.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var exception = await Record.ExceptionAsync(() =>
            _emailService.SendEmailAsync("recipient@example.com", "Test Subject", "<p>Hello</p>"));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public async Task SendEmailAsync_UnsuccessfulResponse_ThrowsSendingEmailException()
    {
        // Arrange
        var response = new Response(HttpStatusCode.BadRequest, null!, null);
        _sendGridClientMock
            .Setup(c => c.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act & Assert
        await Assert.ThrowsAsync<SendingEmailException>(() =>
            _emailService.SendEmailAsync("recipient@example.com", "Test Subject", "<p>Fail</p>"));
    }

    [Fact]
    public async Task SendEmailAsync_ThrowsInnerException_WrappedInSendingEmailException()
    {
        // Arrange
        _sendGridClientMock
            .Setup(c => c.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("Network error"));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<SendingEmailException>(() =>
            _emailService.SendEmailAsync("recipient@example.com", "Test Subject", "<p>Hi</p>"));

        Assert.Contains("Failed to send email", ex.Message);
        Assert.IsType<HttpRequestException>(ex.InnerException);
    }
}
