using System.Data;
using System.Threading.Tasks;
using Npgsql;

namespace Apollo.Data
{
    public interface IDbConnectionFactory
    {
        Task<IDbConnection>  GetConnection();
    }

    public class ConnectionFactory : IDbConnectionFactory
    {
        public async Task<IDbConnection> GetConnection()
        {
            var connection = new NpgsqlConnection(
                "Host=127.0.0.1;Username=apollo_pg;Password=apollo_db_password;Database=apollo_db");
            await connection.OpenAsync();
            return connection;
        }
    }
}