using System.Threading.Tasks;
using Npgsql;

namespace Apollo.Data
{
    public interface IConnectionFactory
    {
        Task<ITestableDbConnection>  GetConnection();
    }

    public class ConnectionFactory : IConnectionFactory
    {
        private const string ConnectionStringTemplate = "Host={0};Username={1};Password={2};Database={3}";
        private readonly IConfiguration config;

        public ConnectionFactory(IConfiguration config)
        {
            this.config = config;
        }

        public async Task<ITestableDbConnection> GetConnection()
        {
            var connectionString = string.Format(
                ConnectionStringTemplate,
                config.DatabaseServer(),
                config.DatabaseUsername(),
                config.DatabasePassword(),
                config.DatabaseName());
            var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();
            return new TestableDbConnection(connection);
        }
    }
}
