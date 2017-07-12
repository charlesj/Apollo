using System.Threading.Tasks;

namespace Apollo.CommandSystem
{
    public interface ICommand
    {
        Task<CommandResult> Execute();
        Task<bool> IsValid();
        Task<bool> Authorize();

        string ClientIpAddress { get; set; }
        string ClientUserAgent { get; set; }
    }
}