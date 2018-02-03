using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.UserSettings
{
    public class GetAllUserSettings : AuthenticatedCommand
    {
        private readonly IUserSettignsDataService dataService;

        public GetAllUserSettings(ILoginService loginService, IUserSettignsDataService dataService) : base(loginService)
        {
            this.dataService = dataService;
        }

        public override async Task<CommandResult> Execute()
        {
            return CommandResult.CreateSuccessResult(await dataService.GetAll());
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(true);
        }
    }
}
