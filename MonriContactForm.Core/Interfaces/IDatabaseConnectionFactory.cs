using Microsoft.Data.SqlClient;

namespace MonriContactForm.Core.Interfaces;

/// <summary>
/// Interface for creating database connections.
/// </summary>
public interface IDatabaseConnectionFactory
{
    /// <summary>
    /// Creates a new SQL database connection.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation, with a <see cref="SqlConnection"/> as the result.</returns>
    Task<SqlConnection> CreateConnectionAsync();
}
