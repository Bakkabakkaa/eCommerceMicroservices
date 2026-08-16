using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace eCommerce.Infrastructure.dbContext;

public class DapperDbContext
{
    public IDbConnection DbConnection => _connection;
    
    private readonly IConfiguration _configuration;
    private readonly IDbConnection _connection;
    public DapperDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
        string? connectionString = _configuration.GetConnectionString("PostgresConnection");
        
        // Create a new NpgsqlConnection with the retrieved connection string
        _connection = new NpgsqlConnection(connectionString);
    }
}