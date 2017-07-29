using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apollo.Data
{
    public abstract class BaseDataService
    {
        protected readonly IConnectionFactory connectionFactory;

        protected BaseDataService(IConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        protected async Task<IReadOnlyList<TResultType>> QueryAsync<TResultType>(string query)
        {
            try
            {
                using (var conn = await connectionFactory.GetConnection())
                {
                    return (await conn.QueryAsync<TResultType>(query)).ToList();
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message, new { query });
                throw new DatabaseException(exception.Message);
            }
        }

        protected async Task<IReadOnlyList<TResultType>> QueryAsync<TResultType>(string query, object parameters)
        {
            try
            {
                using (var conn = await connectionFactory.GetConnection())
                {
                    return (await conn.QueryAsync<TResultType>(query, parameters)).ToList();
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message, new { query, parameters });
                throw new DatabaseException(exception.Message);
            }
        }

        protected async Task<int> Execute(string query, object parameters)
        {
            try
            {
                using (var conn = await connectionFactory.GetConnection())
                {
                    return conn.Execute(query, parameters);
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message, new { query, parameters });
                throw new DatabaseException(exception.Message);
            }
        }
    }
}
