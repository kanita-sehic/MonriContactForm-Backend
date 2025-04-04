using System.Reflection;
using MonriContactForm.Core.Interfaces.Services;
using RazorLight;

namespace MonriContactForm.Infrastructure.Email.Services;

public class EmailTemplateRenderer : IEmailTemplateRenderer
{
    private readonly RazorLightEngine _engine;

    public EmailTemplateRenderer()
    {
        _engine = new RazorLightEngineBuilder()
            .UseEmbeddedResourcesProject(typeof(EmailTemplateRenderer).Assembly)
            .UseMemoryCachingProvider()
            .Build();
    }

    public async Task<string> RenderTemplateAsync<T>(string templateName, T model)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourcePath = $"{assembly.GetName().Name}.Email.Templates.{templateName}.cshtml";

        return await _engine.CompileRenderAsync(resourcePath, model);
    }
}
