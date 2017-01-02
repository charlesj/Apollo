using System.Threading.Tasks;
using Apollo.Commands;

namespace Apollo.Server
{
    public interface IJsonRPCCommandTranslator
    {
        Task<JsonRPCResponse> ExecuteCommand(ICommand command, JsonRPCRequest request);
    }
}