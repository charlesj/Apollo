using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apollo.Data
{
    public interface ILoginSessionDataService
    {
        Task<IReadOnlyList<LoginSession>> GetAllActiveSessions();
        Task CreateSession(string token);
        Task UpdateLastSeen(string token, string ipAddress, string userAgent);
        Task RevokeSession(string token);
    }

    public class LoginSessionDataService: BaseDataService, ILoginSessionDataService
    {
        public const string AllActiveSessionsQuery =
            "select * from login_sessions where revoked is false order by id desc";

        public const string CreateSessionQuery = @"
                    insert into login_sessions(token, created_at, last_seen, ip_address, user_agent, revoked)
                    values (@token, current_timestamp, current_timestamp, 'waiting', 'waiting', false)";

        public const string UpdateLastSeenQuery = @"
                    update login_sessions set last_seen=current_timestamp, ip_address=@ipAddress, user_agent=@userAgent
                    where token=@token";

        public const string RevokeSessionQuery = @"update login_sessions set revoked=true where token=@token";

        public LoginSessionDataService(IConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public async Task<IReadOnlyList<LoginSession>> GetAllActiveSessions()
        {
             return await QueryAsync<LoginSession>(AllActiveSessionsQuery);
        }

        public async Task CreateSession(string token)
        {
            await Execute(CreateSessionQuery, new {token});
        }

        public async Task UpdateLastSeen(string token, string ipAddress, string userAgent)
        {
             await Execute(UpdateLastSeenQuery, new {token, ipAddress, userAgent});
        }

        public async Task RevokeSession(string token)
        {
            await Execute(RevokeSessionQuery, new {token});
        }
    }

    public class LoginSession
    {
        public int id { get; set; }
        public string token { get; set; }
        public DateTime created_at { get; set; }
        public DateTime last_seen { get; set; }
        public string ip_address { get; set; }
        public string user_agent { get; set; }
    }
}
