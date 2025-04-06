using Microsoft.Extensions.Logging;
using MonriContactForm.Core.Entities;
using MonriContactForm.Core.Interfaces.HttpClients;
using MonriContactForm.Core.Interfaces.Repositories;
using MonriContactForm.Core.Interfaces.Services;
using MonriContactForm.Core.Models.Requests;
using MonriContactForm.Core.Models.UsersApi;
using MonriContactForm.Core.Services;
using Moq;

namespace MonriContactForm.Tests.Services;

public class ContactFormServiceTests
{
    private readonly Mock<IUsersRepository> _usersRepositoryMock = new();
    private readonly Mock<IEmailService> _emailServiceMock = new();
    private readonly Mock<IEmailTemplateRenderer> _templateRendererMock = new();
    private readonly Mock<IUsersClient> _usersClientMock = new();
    private readonly Mock<ILogger<ContactFormService>> _loggerMock = new();
    private readonly ContactFormService _service;

    public ContactFormServiceTests()
    {
        _service = new ContactFormService(
            _usersRepositoryMock.Object,
            _emailServiceMock.Object,
            _templateRendererMock.Object,
            _usersClientMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task ContactUserAsync_CreatesNewUser_WhenUserDoesNotExist()
    {
        // Arrange
        var request = GetSampleRequest();

        _usersRepositoryMock
            .Setup(r => r.GetUserByEmailAsync(request.Email))
            .ReturnsAsync((User)null);

        _usersClientMock
            .Setup(c => c.GetUserByEmailAsync(request.Email))
            .ReturnsAsync(new UserDetails());

        _templateRendererMock
            .Setup(t => t.RenderTemplateAsync("UserInfo", It.IsAny<User>()))
            .ReturnsAsync("Email Content");

        // Act
        await _service.ContactUserAsync(request);

        // Assert
        _usersRepositoryMock.Verify(r => r.CreateUserAsync(It.IsAny<User>()), Times.Once);
        _usersRepositoryMock.Verify(r => r.UpdateUserAsync(It.IsAny<User>()), Times.Never);
        _emailServiceMock.Verify(e => e.SendEmailAsync(request.Email, "User Information", "Email Content", null), Times.Once);
    }

    [Fact]
    public async Task ContactUserAsync_UpdateUser_WhenUserExists()
    {
        // Arrange
        var request = GetSampleRequest();

        var existingUser = new User
        {
            FirstName = "OldFirstName",
            LastName = "OldLastName",
            Email = request.Email
        };

        _usersRepositoryMock
            .Setup(r => r.GetUserByEmailAsync(request.Email))
            .ReturnsAsync(existingUser);

        _usersClientMock
            .Setup(c => c.GetUserByEmailAsync(request.Email))
            .ReturnsAsync(new UserDetails());

        _templateRendererMock
            .Setup(t => t.RenderTemplateAsync("UserInfo", It.IsAny<User>()))
            .ReturnsAsync("Email Content");

        // Act
        await _service.ContactUserAsync(request);

        // Assert
        _usersRepositoryMock.Verify(r => r.CreateUserAsync(It.IsAny<User>()), Times.Never);
        _usersRepositoryMock.Verify(r => r.UpdateUserAsync(It.IsAny<User>()), Times.Once);
        _emailServiceMock.Verify(e => e.SendEmailAsync(request.Email, "User Information", "Email Content", null), Times.Once);
    }

    [Fact]
    public async Task ContactUserAsync_UsesExistingUserData_WhenUserDetailsIsNull()
    {
        // Arrange
        var request = GetSampleRequest();

        var existingUser = new User
        {
            Email = request.Email,
            Username = "existingUsername",
            Phone = "123456",
            Address = "Old Address",
            Company = "Old Company",
            Website = "old.com"
        };

        _usersRepositoryMock.Setup(r => r.GetUserByEmailAsync(request.Email)).ReturnsAsync(existingUser);
        _usersClientMock.Setup(c => c.GetUserByEmailAsync(request.Email)).ReturnsAsync((UserDetails)null);
        _templateRendererMock.Setup(t => t.RenderTemplateAsync("UserInfo", It.IsAny<User>())).ReturnsAsync("Email Content");

        // Act
        await _service.ContactUserAsync(request);

        // Assert
        _usersRepositoryMock.Verify(r => r.UpdateUserAsync(It.Is<User>(u =>
            u.Username == "existingUsername" &&
            u.Phone == "123456" &&
            u.Address == "Old Address" &&
            u.Company == "Old Company" &&
            u.Website == "old.com"
        )), Times.Once);
    }

    private ContactUserRequest GetSampleRequest() => new()
    {
        FirstName = "John",
        LastName = "Doe",
        Email = "john@example.com"
    };
}
