using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

namespace Apollo.Data
{
    public interface ITestableDbConnection : IDisposable
    {
        Task<IEnumerable<TResultType>> QueryAsync<TResultType>(string query);
        Task<IEnumerable<TResultType>> QueryAsync<TResultType>(string query, object parameters);
        int Execute(string query, object obj);
        IDbCommand CreateCommand();
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

        public IDbCommand CreateCommand()
        {
            return this.connection.CreateCommand();
        }

        public void Dispose()
        {
            connection?.Dispose();
        }
    }

    public class CountResult
    {
        public int count { get; set; }
    }
}
