using System.Threading.Tasks;
using Apollo.CommandSystem;

namespace Apollo.Commands.Meta
{
    public class TestCommand : ICommand
    {
        public Task<CommandResult> Execute()
        {
            return Task.FromResult(
                new CommandResult { ResultStatus = CommandResultType.Success });
        }

        public Task<bool> Validate()
        {
            return Task.FromResult(true);
        }

        public Task<bool> Authorize()
        {
            return Task.FromResult(true);
        }
    }
}