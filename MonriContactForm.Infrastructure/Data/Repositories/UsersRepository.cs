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

    public async Task<User> CreateUserAsync(User user)
    {
        await using var connection = await _connectionFactory.CreateConnectionAsync();
        var sqlScript = SqlScriptLoader.LoadScript("Users", "Create");

        await using var command = new SqlCommand(sqlScript, connection);
        AddUserQueryParameters(command, user);

        await using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
        {
            // improve
            throw new Exception("User not created");
        }

        return MapUserFromReader(reader);
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        try
        {
            await using var connection = await _connectionFactory.CreateConnectionAsync();
            var sqlScript = SqlScriptLoader.LoadScript("Users", "Update");

            await using var command = new SqlCommand(sqlScript, connection);
            command.Parameters.AddWithValue("@Id", user.Id);
            AddUserQueryParameters(command, user);

            await using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                // improve
                throw new Exception("User not updated");
            }

            return MapUserFromReader(reader);
        }
        catch (Exception e)
        {

            throw;
        }
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

    private void AddUserQueryParameters(SqlCommand command, User user)
    {
        command.Parameters.AddWithValue("@FirstName", user.FirstName);
        command.Parameters.AddWithValue("@LastName", user.LastName);
        command.Parameters.AddWithValue("@Email", user.Email);
        command.Parameters.AddWithValue("@Username", user.Username ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@Address", user.Address ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@Phone", user.Phone ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@Website", user.Website ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@Company", user.Company ?? (object)DBNull.Value);
    }
}
