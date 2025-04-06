using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MonriContactForm.Core.Interfaces.HttpClients;

namespace MonriContactForm.Tests.Integration;

public class UsersClientTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly IUsersClient _usersClient;

    public UsersClientTests(WebApplicationFactory<Program> factory)
    {
        var scope = factory.Services.CreateScope();
        _usersClient = scope.ServiceProvider.GetRequiredService<IUsersClient>();
    }

    [Fact]
    public async Task GetUserByEmailAsync_ReturnsUser_WhenApiReturnsSuccess()
    {
        // Arrange
        var email = "Sincere@april.biz";

        // Act
        var result = await _usersClient.GetUserByEmailAsync(email);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(email, result.Email);
    }

    [Fact]
    public async Task GetUserByEmailAsync_ReturnsNull_WhenApiReturnsNotFound()
    {
        // Arrange
        var email = "notfound@example.com";

        // Act
        var result = await _usersClient.GetUserByEmailAsync(email);

        // Assert
        Assert.Null(result);
    }
}
