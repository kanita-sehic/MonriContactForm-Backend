using System.Data;
using Microsoft.Data.SqlClient;

namespace MonriContactForm.Core.Interfaces;

public interface IDatabaseConnectionFactory
{
    Task<SqlConnection> CreateConnectionAsync();
}
