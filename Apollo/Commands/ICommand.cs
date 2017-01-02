using System.Threading.Tasks;

namespace Apollo.Commands
{
    public interface ICommand
    {
        void Hydrate(object data);
        Task<CommandResult> Execute();
        Task<bool> Validate();
        Task<bool> Authorize();
    }
}