using System.Threading.Tasks;

namespace Apollo.CommandSystem
{
    public abstract class CommandBase : ICommand
    {
        public abstract Task<CommandResult> Execute();

        public abstract Task<bool> IsValid();

        public abstract Task<bool> Authorize();

        public string ClientIpAddress { get; set; }
        public string ClientUserAgent { get; set; }
    }
}