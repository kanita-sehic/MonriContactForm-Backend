using MonriContactForm.Core.Entities;

namespace MonriContactForm.Core.Interfaces.Repositories;

public interface IUsersRepository
{
    Task<User> GetUserByEmailAsync(string email);

    Task<User> CreateUserAsync(User user);

    Task<User> UpdateUserAsync(User user);
}
