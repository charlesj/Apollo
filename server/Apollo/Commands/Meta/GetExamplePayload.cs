using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Services;

namespace Apollo.Commands.Meta
{
    public class GetExamplePayload : AuthenticatedCommand
    {
        private readonly ICommandLocator commandLocator;

        public string command { get; set; }

        public GetExamplePayload(ILoginService loginService, ICommandLocator commandLocator) : base(loginService)
        {
            this.commandLocator = commandLocator;
        }

        public override Task<CommandResult> Execute()
        {
            var cmd = commandLocator.Locate(command);

            return Task.FromResult(CommandResult.CreateSuccessResult(cmd.ExamplePayload()));
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(command));
        }

        public override object ExamplePayload()
        {
            return new {command = "GetExamplePayload"};
        }
    }
}
