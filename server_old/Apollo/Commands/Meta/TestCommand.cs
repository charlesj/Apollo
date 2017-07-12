using System.Threading.Tasks;
using Apollo.CommandSystem;

namespace Apollo.Commands.Meta
{
    public class TestCommand : CommandBase
    {
        public override Task<CommandResult> Execute()
        {
            return Task.FromResult(
                new CommandResult { ResultStatus = CommandResultType.Success, Result = new {test = "Successful Test"}});
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(true);
        }

        public override Task<bool> Authorize()
        {
            return Task.FromResult(true);
        }
    }
}