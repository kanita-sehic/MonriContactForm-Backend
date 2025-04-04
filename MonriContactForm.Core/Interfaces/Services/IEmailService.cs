namespace MonriContactForm.Core.Interfaces.Services;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string htmlContent, string? textContent = null);
}
