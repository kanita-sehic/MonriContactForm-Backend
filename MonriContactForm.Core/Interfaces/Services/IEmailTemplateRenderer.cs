namespace MonriContactForm.Core.Interfaces.Services;

public interface IEmailTemplateRenderer
{
    Task<string> RenderTemplateAsync<T>(string templateName, T model);
}
