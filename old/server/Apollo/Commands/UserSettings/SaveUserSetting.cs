using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.UserSettings
{
    public class SaveUserSetting : AuthenticatedCommand
    {
        private readonly IUserSettignsDataService dataService;

        public string Name { get; set; }
        public string Value { get; set; }

        public SaveUserSetting(ILoginService loginService, IUserSettignsDataService dataService) : base(loginService)
        {
            this.dataService = dataService;
        }

        public override async Task<CommandResult> Execute()
        {
            var setting = new UserSetting {name = Name, value = Value};
            var updated = await dataService.UpdateSetting(setting);
            return CommandResult.CreateSuccessResult(updated);
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(Name) && Value != null);
        }

        public override object ExamplePayload()
        {
            return new
            {
                name = string.Empty, value = string.Empty
            };
        }
    }
}
