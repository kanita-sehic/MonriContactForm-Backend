using System.Text.Json;
using MonriContactForm.Core.Interfaces.HttpClients;
using MonriContactForm.Core.Models.UsersApi;

namespace MonriContactForm.Infrastructure.HttpClients;

public class UsersClient : IUsersClient
{
    private readonly HttpClient _httpClient;

    public UsersClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<UserDetails> GetUserByEmailAsync(string email)
    {

        var response = await _httpClient.GetAsync($"?email={email}");

        if (!response.IsSuccessStatusCode)
        {
            // improve
            return null;
        }

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IList<UserDetails>>(content).FirstOrDefault();
    }
}
