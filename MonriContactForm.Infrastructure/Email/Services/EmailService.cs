using Microsoft.Extensions.Options;
using MonriContactForm.Core.Configuration;
using MonriContactForm.Core.Interfaces.Services;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MonriContactForm.Infrastructure.Email.Services;

public class EmailService : IEmailService
{
    private readonly EmailConfiguration _configuration;

    public EmailService(IOptions<AppSettings> options)
    {
        _configuration = options.Value.Email;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string htmlContent, string? textContent = null)
    {
        try
        {
            var client = new SendGridClient(_configuration.ApiKey);
            var from = new EmailAddress(_configuration.FromEmail, _configuration.FromName);
            var to = new EmailAddress(toEmail);

            var message = MailHelper.CreateSingleEmail(from, to, subject, textContent, htmlContent);

            var response = await client.SendEmailAsync(message);

            if (response.StatusCode is not System.Net.HttpStatusCode.Accepted)
            {
                // throw new EmailSendingException($"Failed to send email: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {

            // throw new EmailSendingException("Email sending failed.", ex);
        }
    }
}
