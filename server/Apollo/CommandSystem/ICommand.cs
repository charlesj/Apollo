using System.Threading.Tasks;

namespace Apollo.CommandSystem
{
    public interface ICommand
    {
        Task<CommandResult> Execute();
        Task<bool> IsValid();
        Task<bool> Authorize();
        object ExamplePayload();

        string ClientIpAddress { get; set; }
        string ClientUserAgent { get; set; }
    }
}
