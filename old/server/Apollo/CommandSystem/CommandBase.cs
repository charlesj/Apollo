using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Apollo.CommandSystem
{
    public abstract class CommandBase : ICommand
    {
        public abstract Task<CommandResult> Execute();

        public abstract Task<bool> IsValid();

        public abstract Task<bool> Authorize();

        public virtual object ExamplePayload()
        {
            return new object();
        }

        [ServerOnly][JsonIgnore]
        public string ClientIpAddress { get; set; }

        [ServerOnly][JsonIgnore]
        public string ClientUserAgent { get; set; }
    }
}
