using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace Apollo.Data
{
    public interface ILoginSessionDataService
    {
        Task<IReadOnlyList<LoginSession>> GetAllSessions();
        Task CreateSession(string token);
        Task UpdateLastSeen(string token);
        Task DeleteSession(string token);
    }

    public class LoginSessionDataService: ILoginSessionDataService
    {
        private readonly IDbConnectionFactory connectionFactory;

        public LoginSessionDataService(IDbConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public async Task<IReadOnlyList<LoginSession>> GetAllSessions()
        {
            using (var connection = await connectionFactory.GetConnection())
            {
                var query = "select * from login_sessions order by id desc";
                var results = await connection.QueryAsync<LoginSession>(query);
                return results.ToList();
            }
        }

        public async Task CreateSession(string token)
        {
            using (var connection = await connectionFactory.GetConnection())
            {
                connection.Execute(@"
                    insert into login_sessions(token, created_at, last_seen)
                    values (@token, current_timestamp, current_timestamp)",
                    new {token});
            }
        }

        public async Task UpdateLastSeen(string token)
        {
            using (var connection = await connectionFactory.GetConnection())
            {
                connection.Execute(@"
                    update login_sessions set last_seen=current_timestamp
                    where token=@token",
                    new {token});
            }
        }

        public async Task DeleteSession(string token)
        {
            using (var connection = await connectionFactory.GetConnection())
            {
                connection.Execute(@"delete from login_sessions
                                     where token=@token", new {token});
            }
        }
    }

    public class LoginSession
    {
        public int id { get; set; }
        public string token { get; set; }
        public DateTime created_at { get; set; }
        public DateTime last_seen { get; set; }
    }
}