using System.Threading.Tasks;
using Apollo.Commands;

namespace Apollo.Server
{
    public interface IJsonRpcCommandTranslator
    {
        Task<JsonRpcResponse> ExecuteCommand(ICommand command, JsonRpcRequest request);
    }
}