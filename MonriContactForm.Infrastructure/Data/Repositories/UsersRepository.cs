using Microsoft.Data.SqlClient;
using MonriContactForm.Core.Entities;
using MonriContactForm.Core.Interfaces;
using MonriContactForm.Core.Interfaces.Repositories;

namespace MonriContactForm.Infrastructure.Data.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly IDatabaseConnectionFactory _connectionFactory;

    public UsersRepository(IDatabaseConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public Task<User> CreateUserAsync(User user)
    {
        throw new NotImplementedException();
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        await using var connection = await _connectionFactory.CreateConnectionAsync();
        var sqlScript = SqlScriptLoader.LoadScript("Users", "GetByEmail");

        await using var command = new SqlCommand(sqlScript, connection);
        command.Parameters.AddWithValue("@Email", email);

        await using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
        {
            return default;
        }

        return MapUserFromReader(reader);
    }

    private User MapUserFromReader(SqlDataReader reader)
    {
        return new User
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id")),
            FirstName = reader["FirstName"] as string,
            LastName = reader["LastName"] as string,
            Username = reader["Username"] as string,
            Address = reader["Address"] as string,
            Email = reader["Email"] as string,
            Phone = reader["Phone"] as string,
            Website = reader["Website"] as string,
            Company = reader["Company"] as string
        };
    }
}
