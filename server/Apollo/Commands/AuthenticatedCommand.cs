using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Services;

namespace Apollo.Commands
{
    public abstract class AuthenticatedCommand : ICommand
    {
        protected readonly ILoginService loginService;

        protected AuthenticatedCommand(ILoginService loginService)
        {
            this.loginService = loginService;
        }

        public string Token { get; set; }

        public abstract Task<CommandResult> Execute();

        public abstract Task<bool> IsValid();

        public Task<bool> Authorize()
        {
            return loginService.ValidateToken(this.Token);
        }
    }
}