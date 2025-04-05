namespace MonriContactForm.Core.Interfaces.Services;

/// <summary>
/// Interface for rendering email templates with dynamic content.
/// </summary>
public interface IEmailTemplateRenderer
{
    /// <summary>
    /// Renders an email template with the provided model data.
    /// </summary>
    /// <typeparam name="T">The type of the model to pass to the template.</typeparam>
    /// <param name="templateName">The name of the template to render.</param>
    /// <param name="model">The model containing data to inject into the template.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation, with the rendered template as the result.</returns>
    Task<string> RenderTemplateAsync<T>(string templateName, T model);
}