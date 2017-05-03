using System.Threading.Tasks;
using Apollo.Data;
using Apollo.Utilities;

namespace Apollo.Services
{
    public interface IUserSettingsService
    {
        Task SetSetting<TSettingType>(string name, TSettingType value);
        Task<TSettingValue> GetSetting<TSettingValue>(string name);
    }

    public class UserSettingsService : IUserSettingsService
    {
        private readonly IJsonSerializer serializer;
        private readonly IUserSettignsDataService userSettignsDataService;

        public UserSettingsService(IJsonSerializer serializer, IUserSettignsDataService userSettignsDataService)
        {
            this.serializer = serializer;
            this.userSettignsDataService = userSettignsDataService;
        }
        public async Task SetSetting<TSettingType>(string name, TSettingType value)
        {
            var serialized = this.serializer.Serialize(value);
            await this.userSettignsDataService.UpsertSetting(new UserSetting {Name = name, Value = serialized});
        }

        public async Task<TSettingValue> GetSetting<TSettingValue>(string name)
        {
            var setting = await this.userSettignsDataService.GetUserSetting(name);
            return this.serializer.Deserialize<TSettingValue>(setting.Value);
        }
    }
}