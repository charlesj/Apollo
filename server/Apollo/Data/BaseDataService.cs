using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apollo.Data
{
    public interface ITableModel
    {
        int id { get; set; }
    }

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

        protected async Task<TModel> Upsert<TModel>(string insertQuery, string updateQuery, string selectQuery, TModel parameters) where TModel : ITableModel
        {
            int id = parameters.id;
            if (id == default(int))
            {
                id = await InsertAndReturnId(insertQuery, parameters);
            }
            else
            {
                await Execute(updateQuery, parameters);
            }

            return (await QueryAsync<TModel>(selectQuery, new {id})).Single();
        }

        protected async Task<int> InsertAndReturnId(string query, object parameters)
        {
            try
            {
                using (var conn = await connectionFactory.GetConnection())
                {
                    return (await conn.QueryAsync<IdResult>(query, parameters)).Single().id;
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message, new { query, parameters });
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
