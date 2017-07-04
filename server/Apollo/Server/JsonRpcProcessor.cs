using System.Threading.Tasks;
using Apollo.CommandSystem;

namespace Apollo.Server
{
    public interface IJsonRpcProcessor
    {
        Task<HttpResponse> Process(string request, HttpClientInfo clientInfo);
    }

    public class JsonRpcProcessor : IJsonRpcProcessor
    {
        private readonly ICommandLocator commandLocator;
        private readonly IJsonRpcRequestParser requestParser;
        private readonly IJsonRpcCommandTranslator translator;
        private readonly IJsonRpcHttpConverter converter;

        public JsonRpcProcessor(
            ICommandLocator commandLocator,
            IJsonRpcCommandTranslator translator,
            IJsonRpcHttpConverter converter,
            IJsonRpcRequestParser requestParser)
        {
            this.commandLocator = commandLocator;
            this.requestParser = requestParser;
            this.translator = translator;
            this.converter = converter;
        }

        public async Task<HttpResponse> Process(string request, HttpClientInfo clientInfo)
        {
            Logger.Info($"REQ RECIEVED");
            if (string.IsNullOrWhiteSpace(request))
            {
                Logger.Error("BAD REQ - Empty Command");
                return HttpResponse.BadRequest();
            }

            var parsedRequest = requestParser.Parse(request);
            Logger.Info("REQ PARSED", parsedRequest);
            if (!parsedRequest.Success)
            {
                Logger.Error("Could not parse request", parsedRequest);
                return HttpResponse.BadRequest("Could not parse RPC Request");
            }

            var command = commandLocator.Locate(parsedRequest.Request.Method);
            if (command == null)
            {
                Logger.Error("Could not locate method", parsedRequest);
                return HttpResponse.NotFound("Could not locate requested method");
            }

            Logger.Trace("command located", command);

            var response = await translator.ExecuteCommand(command, parsedRequest.Request, clientInfo);

            Logger.Info("REQ COMPLETE", response);
            return converter.Convert(response);
        }
    }
}
