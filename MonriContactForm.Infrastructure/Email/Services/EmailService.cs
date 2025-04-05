using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MonriContactForm.Core.Configuration;
using MonriContactForm.Core.Exceptions;
using MonriContactForm.Core.Interfaces.Services;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MonriContactForm.Infrastructure.Email.Services;

public class EmailService : IEmailService
{
    private readonly EmailConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<AppSettings> options, ILogger<EmailService> logger)
    {
        _configuration = options.Value.Email;
        _logger = logger;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string htmlContent, string? textContent = null)
    {
        try
        {
            _logger.LogInformation($"Sending email to {toEmail}.");

            var client = new SendGridClient(_configuration.ApiKey);
            var from = new EmailAddress(_configuration.FromEmail, _configuration.FromName);
            var to = new EmailAddress(toEmail);

            var message = MailHelper.CreateSingleEmail(from, to, subject, textContent, htmlContent);

            var response = await client.SendEmailAsync(message);

            if (response.StatusCode is not HttpStatusCode.Accepted or HttpStatusCode.OK)
            {
                _logger.LogError($"Failed to send email to the address {toEmail}. Status code: {response.StatusCode}. Reason: {response.Body.ReadAsStringAsync()}");
                throw new SendingEmailException($"Failed to send email to the address {toEmail}.");
            }
        }
        catch (Exception ex)
        {
            throw new SendingEmailException($"Failed to send email to the address {toEmail}.", ex);
        }
    }
}
