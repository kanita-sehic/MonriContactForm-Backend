using MonriContactForm.Core.Entities;

namespace MonriContactForm.Core.Interfaces.Repositories;

/// <summary>
/// Interface for performing CRUD operations on the user data in the database.
/// </summary>
public interface IUsersRepository
{
    /// <summary>
    /// Retrieves a user by their email address.
    /// </summary>
    /// <param name="email">The email address of the user to retrieve.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation, with a <see cref="User"/> as the result.</returns>
    Task<User> GetUserByEmailAsync(string email);

    /// <summary>
    /// Creates a new user in the database.
    /// </summary>
    /// <param name="user">The user to create.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation, with the created <see cref="User"/> as the result.</returns>
    Task<User> CreateUserAsync(User user);

    /// <summary>
    /// Updates an existing user in the database.
    /// </summary>
    /// <param name="user">The user to update.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation, with the updated <see cref="User"/> as the result.</returns>
    Task<User> UpdateUserAsync(User user);
}
