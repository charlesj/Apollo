using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace Apollo.Data
{
    public interface ITestableDbConnection : IDisposable
    {
        Task<IEnumerable<TResultType>> QueryAsync<TResultType>(string query);
        Task<IEnumerable<TResultType>> QueryAsync<TResultType>(string query, object parameters);
        int Execute(string query, object obj);
    }

    public class TestableDbConnection : ITestableDbConnection
    {
        private readonly IDbConnection connection;

        public TestableDbConnection(IDbConnection connection)
        {
            this.connection = connection;
        }

        public Task<IEnumerable<TResultType>> QueryAsync<TResultType>(string query)
        {
            return this.connection.QueryAsync<TResultType>(query);
        }

        public Task<IEnumerable<TResultType>> QueryAsync<TResultType>(string query, object parameters)
        {
            return this.connection.QueryAsync<TResultType>(query, parameters);
        }

        public int Execute(string query, object obj)
        {
            return this.connection.Execute(query, obj);
        }

        public void Dispose()
        {
            connection?.Dispose();
        }
    }
}