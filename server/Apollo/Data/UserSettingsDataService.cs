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
        public const string UserSettingSql = "select * from user_settings where name=@name";

        public const string UpdateSettingSql =
            "update user_settings set value=@newValue, updated_at=current_timestamp where name=@name";

        public UserSettingsDataService(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public async Task<UserSetting> GetUserSetting(string name)
        {
            var results = await QueryAsync<UserSetting>(UserSettingSql, new { name });
            if(!results.Any())
                throw new InvalidOperationException($"Could not find user setting '{name}'");
            if(results.Count > 1)
                throw new InvalidOperationException($"Multiple values for '{name}' - fix the database");

            return results[0];
        }

        public async Task UpdateSetting(UserSetting setting)
        {
            await Execute(UpdateSettingSql, new {setting.name, newValue = setting.value});
        }
    }

    public class UserSetting
    {
        public string name { get; set; }
        public string value { get; set; }
    }
}
