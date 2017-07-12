using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Services;

namespace Apollo.Commands
{
    public abstract class AuthenticatedCommand : CommandBase
    {
        protected readonly ILoginService loginService;

        protected AuthenticatedCommand(ILoginService loginService)
        {
            this.loginService = loginService;
        }

        public string Token { get; set; }

        public abstract override Task<CommandResult> Execute();

        public abstract override Task<bool> IsValid();

        public override async Task<bool> Authorize()
        {
            return await this.loginService.ValidateToken(this.Token, this.ClientIpAddress, this.ClientUserAgent);
        }
    }
}
