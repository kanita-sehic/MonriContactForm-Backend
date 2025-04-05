using MonriContactForm.Core.Models.UsersApi;

namespace MonriContactForm.Core.Interfaces.HttpClients;

/// <summary>
/// Interface for interacting with the user data in an external service.
/// </summary>
public interface IUsersClient
{
    /// <summary>
    /// Retrieves user details by email address.
    /// </summary>
    /// <param name="email">The email address of the user.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation, with a <see cref="UserDetails"/> as the result.</returns>
    Task<UserDetails> GetUserByEmailAsync(string email);
}
