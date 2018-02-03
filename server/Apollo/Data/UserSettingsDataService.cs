using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apollo.Data
{
    public interface IUserSettignsDataService
    {
        Task<UserSetting> GetUserSetting(string name);
        Task<UserSetting> UpdateSetting(UserSetting setting);
        Task<IReadOnlyList<UserSetting>> GetAll();
    }

    public class UserSettingsDataService : BaseDataService, IUserSettignsDataService
    {
        public const string UserSettingSql = "select * from user_settings where name=@name";

        public const string UpdateSettingSql =
            "update user_settings set value=@newValue, updated_at=current_timestamp where name=@name";

        public const string InsertSettingsSql = "insert into user_settings (name, value, created_at, updated_at) " +
                                                "values (@name, @value, current_timestamp, current_timestamp)";

        public UserSettingsDataService(IConnectionFactory connectionFactory) : base(connectionFactory)
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

        public async Task<UserSetting> UpdateSetting(UserSetting setting)
        {
            var current = await QueryAsync<UserSetting>(UserSettingSql, setting);
            if (current.Count == 0)
                await Execute(InsertSettingsSql, setting);
            else
                await Execute(UpdateSettingSql, new {setting.name, newValue = setting.value});
            current = await QueryAsync<UserSetting>(UserSettingSql, setting);
            return current.Single();
        }

        public Task<IReadOnlyList<UserSetting>> GetAll()
        {
            return QueryAsync<UserSetting>("select * from user_settings where name != 'password_hash'");
        }
    }

    public class UserSetting
    {
        public int id { get; set; }
        public string name { get; set; }
        public string value { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
