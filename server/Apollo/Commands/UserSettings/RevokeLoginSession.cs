using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Services;

namespace Apollo.Commands.UserSettings
{
    public class RevokeLoginSession: AuthenticatedCommand
    {
        public RevokeLoginSession(ILoginService loginService) : base(loginService)
        {
        }

        public string TokenToRevoke { get; set; }

        public override async Task<CommandResult> Execute()
        {
            await this.loginService.RevokeToken(this.TokenToRevoke);
            return CommandResult.SuccessfulResult;
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(this.TokenToRevoke));
        }
    }
}
