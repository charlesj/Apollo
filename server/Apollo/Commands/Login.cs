using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Services;

namespace Apollo.Commands
{
    public class Login : ICommand
    {
        private readonly ILoginService loginService;

        public Login(ILoginService loginService)
        {
            this.loginService = loginService;
        }

        public string Password { get; set; }

        public async Task<CommandResult> Execute()
        {
            var result = await loginService.Authenticate(this.Password);

            return new CommandResult()
            {
                Result = new LoginResult{ token = result },
                ResultStatus = string.IsNullOrEmpty(result) ? CommandResultType.Error : CommandResultType.Success
            };
        }

        public Task<bool> IsValid()
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(this.Password));
        }

        public Task<bool> Authorize()
        {
            return Task.FromResult(true);
        }

        public class LoginResult
        {
            public string token { get; set; }
        }
    }
}