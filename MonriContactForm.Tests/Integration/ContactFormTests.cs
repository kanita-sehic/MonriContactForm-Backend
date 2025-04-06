using System.Net;
using System.Net.Http.Json;
using MonriContactForm.Core.Models.Requests;


namespace MonriContactForm.Tests.Integration;
public class ContactFormTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ContactFormTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task ContactUser_ReturnsOk()
    {
        // Arrange
        var request = new ContactUserRequest
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/contact-form", request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }


    [Fact]
    public async Task ContactForm_MissingRequiredFields_ReturnsBadRequest()
    {
        // Arrange
        var request = new ContactUserRequest();

        // Act
        var response = await _client.PostAsJsonAsync("/contact-form", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task ContactForm_RateLimitEnforced_ReturnsTooManyRequests()
    {
        // Arrange
        var request = new ContactUserRequest
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com"
        };

        // Act
        var firstResponse = await _client.PostAsJsonAsync("/contact-form", request);
        var secondResponse = await _client.PostAsJsonAsync("/contact-form", request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, firstResponse.StatusCode);
        Assert.Equal(HttpStatusCode.TooManyRequests, secondResponse.StatusCode);
    }

}
