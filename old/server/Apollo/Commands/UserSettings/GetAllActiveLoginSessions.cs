using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.UserSettings
{
    public class GetAllActiveLoginSessions: AuthenticatedCommand
    {
        private readonly ILoginSessionDataService loginSessionDataService;

        public GetAllActiveLoginSessions(
            ILoginService loginService,
            ILoginSessionDataService loginSessionDataService) : base(loginService)
        {
            this.loginSessionDataService = loginSessionDataService;
        }

        public override async Task<CommandResult> Execute()
        {
            var activeSessions = await this.loginSessionDataService.GetAllActiveSessions();
            return CommandResult.CreateSuccessResult(activeSessions);
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(true);
        }
    }
}
