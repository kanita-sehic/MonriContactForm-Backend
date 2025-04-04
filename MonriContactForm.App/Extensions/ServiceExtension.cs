using AspNetCoreRateLimit;
using MonriContactForm.Core.Interfaces;
using MonriContactForm.Core.Interfaces.HttpClients;
using MonriContactForm.Core.Interfaces.Repositories;
using MonriContactForm.Core.Interfaces.Services;
using MonriContactForm.Core.Services;
using MonriContactForm.Infrastructure.Data;
using MonriContactForm.Infrastructure.Data.Repositories;
using MonriContactForm.Infrastructure.Email.Services;
using MonriContactForm.Infrastructure.HttpClients;

namespace MonriContactForm.App.Extensions;

public static class ServiceExtension
{
    public static void RegisterDatabaseConnectionFactory(this IServiceCollection services, IConfiguration configuration)
    {
        var sqlConnectionString = configuration.GetConnectionString("SqlConnection");
        services.AddSingleton<IDatabaseConnectionFactory>(new DatabaseConnectionFactory(sqlConnectionString));
    }
    public static void RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUsersRepository, UsersRepository>();
    }

    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IContactFormService, ContactFormService>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<IEmailTemplateRenderer, EmailTemplateRenderer>();
    }

    public static void RegisterHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        var usersApiUrl = configuration.GetValue<string>("UsersApiUrl");

        services.AddHttpClient<IUsersClient, UsersClient>(client =>
        {
            client.BaseAddress = new Uri(usersApiUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });
    }

    public static void ConfigureRateLimiting(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache();
        services.AddInMemoryRateLimiting();
        services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
    }
}
