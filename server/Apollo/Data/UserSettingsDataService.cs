using System;
using System.Linq;
using System.Threading.Tasks;

namespace Apollo.Data
{
    public interface IUserSettignsDataService
    {
        Task<UserSetting> GetUserSetting(string name);
        Task UpdateSetting(UserSetting setting);
    }

    public class UserSettingsDataService : BaseDataService, IUserSettignsDataService
    {
        public UserSettingsDataService(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
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

        public async Task UpdateSetting(UserSetting setting)
        {
            using (var connection = await connectionFactory.GetConnection())
            {
                connection.Execute(
                    "update user_settings set value=@newValue, updated_at=current_timestamp where name=@name",
                    new {name = setting.name, newValue = setting.value});
            }
        }
    }

    public class UserSetting
    {
        public string name { get; set; }
        public string value { get; set; }
    }
}