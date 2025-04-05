using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using MonriContactForm.Core.Entities;
using MonriContactForm.Core.Interfaces;
using MonriContactForm.Core.Interfaces.Repositories;

namespace MonriContactForm.Infrastructure.Data.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly IDatabaseConnectionFactory _connectionFactory;
    private readonly ILogger<UsersRepository> _logger;

    public UsersRepository(IDatabaseConnectionFactory connectionFactory, ILogger<UsersRepository> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<User> CreateUserAsync(User user)
    {
        try
        {
            _logger.LogInformation($"Creating user with the following data: {user}.");

            await using var connection = await _connectionFactory.CreateConnectionAsync();
            var sqlScript = SqlScriptLoader.LoadScript("Users", "Create");

            await using var command = new SqlCommand(sqlScript, connection);
            AddUserQueryParameters(command, user);

            await using var reader = await command.ExecuteReaderAsync();
            await reader.ReadAsync();

            return MapUserFromReader(reader);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error while creating user: {user}.");
            throw;
        }
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        try
        {
            _logger.LogInformation($"Updating user with email {user.Email} with the following data: {user}.");

            await using var connection = await _connectionFactory.CreateConnectionAsync();
            var sqlScript = SqlScriptLoader.LoadScript("Users", "Update");

            await using var command = new SqlCommand(sqlScript, connection);
            command.Parameters.AddWithValue("@Id", user.Id);
            AddUserQueryParameters(command, user);

            await using var reader = await command.ExecuteReaderAsync();
            await reader.ReadAsync();

            return MapUserFromReader(reader);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error while updating user: {user}.");
            throw;
        }
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        try
        {
            _logger.LogInformation($"Fetching user with email: {email}.");

            await using var connection = await _connectionFactory.CreateConnectionAsync();
            var sqlScript = SqlScriptLoader.LoadScript("Users", "GetByEmail");

            await using var command = new SqlCommand(sqlScript, connection);
            command.Parameters.AddWithValue("@Email", email);

            await using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                _logger.LogInformation($"There is no existing user with email: {email}.");
                return default;
            }

            return MapUserFromReader(reader);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error while fetching user with email: {email}.");
            throw;
        }
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
