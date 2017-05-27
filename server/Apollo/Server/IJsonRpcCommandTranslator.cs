using System.Threading.Tasks;
using Apollo.CommandSystem;

namespace Apollo.Server
{
    public interface IJsonRpcCommandTranslator
    {
        Task<JsonRpcResponse> ExecuteCommand(ICommand command, JsonRpcRequest request, HttpClientInfo clientInfo);
    }
}