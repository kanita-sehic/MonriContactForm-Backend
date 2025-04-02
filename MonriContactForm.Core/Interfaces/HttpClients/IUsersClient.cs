using MonriContactForm.Core.Models.UsersApi;

namespace MonriContactForm.Core.Interfaces.HttpClients;

public interface IUsersClient
{
    Task<UserDetails> GetUserByEmailAsync(string email);
}
