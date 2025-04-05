using System.Text.Json;
using Microsoft.Extensions.Logging;
using MonriContactForm.Core.Interfaces.HttpClients;
using MonriContactForm.Core.Models.UsersApi;

namespace MonriContactForm.Infrastructure.HttpClients;

public class UsersClient : IUsersClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<UsersClient> _logger;

    public UsersClient(HttpClient httpClient, ILogger<UsersClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<UserDetails> GetUserByEmailAsync(string email)
    {
        _logger.LogInformation($"Making and HTTP request to fetch user details by email: {email}.");

        var response = await _httpClient.GetAsync($"?email={email}");

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogInformation($"There are no details about user with email: {email}. Status code: {response.StatusCode}. Reason: {response.ReasonPhrase}");
            return default;
        }

        _logger.LogInformation($"User details fetched successfully for email: {email}.");

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IList<UserDetails>>(content).FirstOrDefault();
    }
}
