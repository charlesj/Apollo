using System.Threading.Tasks;
using Apollo.CommandSystem;

namespace Apollo.Commands.Meta
{
    public class TestCommand : ICommand
    {
        public Task<CommandResult> Execute()
        {
            return Task.FromResult(
                new CommandResult { ResultStatus = CommandResultType.Success, Result = new {test = "Successful Test"}});
        }

        public Task<bool> IsValid()
        {
            return Task.FromResult(true);
        }

        public Task<bool> Authorize()
        {
            return Task.FromResult(true);
        }
    }
}