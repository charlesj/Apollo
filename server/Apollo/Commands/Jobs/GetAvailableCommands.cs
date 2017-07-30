using System.Linq;
using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Services;

namespace Apollo.Commands.Jobs
{
    public class GetAvailableCommands : AuthenticatedCommand
    {
        private readonly ICommandLocator commandLocator;

        public GetAvailableCommands(ICommandLocator commandLocator,ILoginService loginService) : base(loginService)
        {
            this.commandLocator = commandLocator;
        }

        public override async Task<CommandResult> Execute()
        {
            await Task.CompletedTask;
            var allCommands = commandLocator.GetAllAvailableCommands();

            return CommandResult.CreateSuccessResult(allCommands.Select(c => c.Name.ToLowerInvariant()));
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(true);
        }
    }
}
