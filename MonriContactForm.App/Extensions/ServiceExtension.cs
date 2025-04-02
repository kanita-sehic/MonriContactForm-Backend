using MonriContactForm.Core.Interfaces;
using MonriContactForm.Core.Interfaces.HttpClients;
using MonriContactForm.Core.Interfaces.Repositories;
using MonriContactForm.Core.Interfaces.Services;
using MonriContactForm.Core.Services;
using MonriContactForm.Infrastructure.Data;
using MonriContactForm.Infrastructure.Data.Repositories;
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
    }

    public static void RegisterHttpClients(this IServiceCollection services)
    {
        // add base address
        services.AddHttpClient<IUsersClient, UsersClient>();
    }
}
