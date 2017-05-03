using System;
using System.Linq;
using System.Threading.Tasks;

namespace Apollo.Data
{
    public interface IUserSettignsDataService
    {
        Task<UserSetting> GetUserSetting(string name);
        Task UpsertSetting(UserSetting setting);
    }

    public class UserSettingsDataService : IUserSettignsDataService
    {
        private readonly IDbConnectionFactory connectionFactory;

        public UserSettingsDataService(IDbConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public async Task<UserSetting> GetUserSetting(string name)
        {
            using (var connection = await connectionFactory.GetConnection())
            {
                var query = "select * from user_settings where name=@name";
                var results = (await connection.QueryAsync<UserSetting>(query, new { name })).ToList();
                if(!results.Any())
                    throw new InvalidOperationException($"Could not find user setting '{name}'");
                if(results.Count > 1)
                    throw new InvalidOperationException($"Multiple values for '{name}' - fix the database");

                return results[0];
            }
        }

        public Task UpsertSetting(UserSetting setting)
        {
            throw new System.NotImplementedException();
        }
    }

    public class UserSetting
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}