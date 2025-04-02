using MonriContactForm.Core.Entities;

namespace MonriContactForm.Core.Interfaces.Repositories;

public interface IUsersRepository
{
    Task<User> CreateUserAsync(User user);

    Task<User?> GetUserByEmailAsync(string email);
}
