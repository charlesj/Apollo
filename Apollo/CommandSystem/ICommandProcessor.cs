using System.Threading.Tasks;

namespace Apollo.CommandSystem
{
    public interface ICommandProcessor
    {
        Task<CommandResult> Process(ICommand command, object parameters);
    }
}