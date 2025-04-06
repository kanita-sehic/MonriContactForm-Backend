using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MonriContactForm.Core.Entities;
using MonriContactForm.Core.Interfaces.HttpClients;
using MonriContactForm.Core.Interfaces.Repositories;
using MonriContactForm.Core.Interfaces.Services;
using MonriContactForm.Core.Models.UsersApi;
using Moq;

namespace MonriContactForm.Tests.Integration;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            MockUsersClient(services);
            MockUsersRepository(services);
            MockEmailTemplateRenderer(services);
            MockEmailService(services);
        });
    }

    private void MockUsersClient(IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IUsersClient));

        if (descriptor is not null)
        {
            services.Remove(descriptor);
        }

        var mockUsersClient = new Mock<IUsersClient>();

        mockUsersClient
                .Setup(client => client.GetUserByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new UserDetails { });

        services.AddSingleton(mockUsersClient.Object);
    }

    private void MockUsersRepository(IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IUsersRepository));

        if (descriptor is not null)
        {
            services.Remove(descriptor);
        }

        var mockUsersRepository = new Mock<IUsersRepository>();

        mockUsersRepository
            .Setup(repo => repo.GetUserByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((User)null);

        mockUsersRepository
            .Setup(repo => repo.CreateUserAsync(It.IsAny<User>()))
            .ReturnsAsync((User user) => user);

        mockUsersRepository
            .Setup(repo => repo.UpdateUserAsync(It.IsAny<User>()))
            .ReturnsAsync((User user) => user);

        services.AddSingleton(mockUsersRepository.Object);
    }

    private void MockEmailTemplateRenderer(IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IEmailTemplateRenderer));

        if (descriptor is not null)
        {
            services.Remove(descriptor);
        }

        var mockEmailTemplateRenderer = new Mock<IEmailTemplateRenderer>();

        mockEmailTemplateRenderer
            .Setup(renderer => renderer.RenderTemplateAsync(It.IsAny<string>(), It.IsAny<object>()))
            .ReturnsAsync("Mocked Email Content");

        services.AddSingleton(mockEmailTemplateRenderer.Object);
    }

    private void MockEmailService(IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IEmailService));

        if (descriptor is not null)
        {
            services.Remove(descriptor);
        }

        var mockEmailService = new Mock<IEmailService>();

        mockEmailService
            .Setup(service => service.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), null))
            .Returns(Task.CompletedTask);

        services.AddSingleton(mockEmailService.Object);
    }
}
