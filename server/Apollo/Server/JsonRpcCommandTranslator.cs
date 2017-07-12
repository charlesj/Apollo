using System.Threading.Tasks;
using Apollo.CommandSystem;

namespace Apollo.Server
{
    public class JsonRpcCommandTranslator : IJsonRpcCommandTranslator
    {
        private readonly ICommandProcessor processor;

        public JsonRpcCommandTranslator(ICommandProcessor processor)
        {
            this.processor = processor;
        }

        public async Task<JsonRpcResponse> ExecuteCommand(ICommand command, JsonRpcRequest request, HttpClientInfo clientInfo)
        {
            command.ClientIpAddress = clientInfo.IpAddress;
            command.ClientUserAgent = clientInfo.Agent;
            var commandResult = await processor.Process(command, request.Params);
            return new JsonRpcResponse
            {
                id = request.Id,
                result = commandResult,
                error = commandResult.ErrorMessage
            };
        }
    }
}