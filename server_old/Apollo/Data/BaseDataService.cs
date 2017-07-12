using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apollo.Data
{
    public abstract class BaseDataService
    {
        protected readonly IDbConnectionFactory connectionFactory;

        protected BaseDataService(IDbConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        protected async Task<IReadOnlyList<TResultType>> QueryAsync<TResultType>(string query)
        {
            using (var conn = await connectionFactory.GetConnection())
            {
                return (await conn.QueryAsync<TResultType>(query)).ToList();
            }
        }

        protected async Task<IReadOnlyList<TResultType>> QueryAsync<TResultType>(string query, object parameters)
        {
            using (var conn = await connectionFactory.GetConnection())
            {
                return (await conn.QueryAsync<TResultType>(query, parameters)).ToList();
            }
        }

        protected async Task<int> Execute(string query, object obj)
        {
            using (var conn = await connectionFactory.GetConnection())
            {
                return conn.Execute(query, obj);
            }
        }
    }
}
