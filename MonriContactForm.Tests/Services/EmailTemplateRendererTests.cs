using MonriContactForm.Core.Entities;
using MonriContactForm.Infrastructure.Email.Services;
using RazorLight;

namespace MonriContactForm.Tests.Services;

public class EmailTemplateRendererTests
{
    private readonly EmailTemplateRenderer _renderer;

    public EmailTemplateRendererTests()
    {
        _renderer = new EmailTemplateRenderer();
    }

    [Fact]
    public async Task RenderTemplateAsync_UserInfoTemplate_WithAllUserDataPopulated()
    {
        // Arrange
        var user = new User
        {
            FirstName = "Test",
            LastName = "User",
            Email = "test@email.com",
            Username = "testuser",
            Phone = "123456789",
            Address = "123 Test St",
            Company = "Test Company",
            Website = "www.test.com"
        };

        // Act
        var result = await _renderer.RenderTemplateAsync("UserInfo", user);

        // Assert
        Assert.Contains(user.FirstName, result);
        Assert.Contains(user.LastName, result);
        Assert.Contains(user.Email, result);
        Assert.Contains(user.Username, result);
        Assert.Contains(user.Phone, result);
        Assert.Contains(user.Address, result);
        Assert.Contains(user.Company, result);
        Assert.Contains(user.Website, result);
    }

    [Fact]
    public async Task RenderTemplateAsync_UserInfoTemplate_WithPartialDataPopulated()
    {
        // Arrange
        var user = new User
        {
            FirstName = "Test",
            LastName = "User",
            Email = "test@email.com"
        };

        // Act
        var result = await _renderer.RenderTemplateAsync("UserInfo", user);

        // Assert
        Assert.Contains("Name", result);
        Assert.Contains("Email", result);
        Assert.DoesNotContain("Username", result);
        Assert.DoesNotContain("Phone", result);
        Assert.DoesNotContain("Company", result);
        Assert.DoesNotContain("Website", result);
        Assert.DoesNotContain("Address", result);
    }

    [Fact]
    public async Task RenderTemplateAsync_ShouldThrow_WhenTemplateNotFound()
    {
        // Arrange
        var user = new User
        {
            FirstName = "Test",
            LastName = "User",
            Email = "test@email.com"
        };

        // Act & Assert
        await Assert.ThrowsAsync<TemplateNotFoundException>(() =>
            _renderer.RenderTemplateAsync("NonExistentTemplate", user));
    }
}
