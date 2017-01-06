using System;
using System.Threading.Tasks;
using Apollo.CommandSystem;

namespace Apollo.Server
{
    public interface IJsonRpcProcessor
    {
        Task<HttpResponse> Process(string request);
    }

    public class JsonRpcProcessor : IJsonRpcProcessor
    {
        private readonly ICommandLocator commandLocator;
        private readonly IJsonRpcRequestParser requestParser;
        private readonly IJsonRpcCommandTranslator translator;
        private readonly IJsonRpcHttpConverter converter;
        private readonly IJsonRpcRequestLogger logger;

        public JsonRpcProcessor(
            ICommandLocator commandLocator,
            IJsonRpcCommandTranslator translator,
            IJsonRpcHttpConverter converter,
            IJsonRpcRequestLogger logger,
            IJsonRpcRequestParser requestParser)
        {
            this.commandLocator = commandLocator;
            this.requestParser = requestParser;
            this.translator = translator;
            this.converter = converter;
            this.logger = logger;
        }

        public async Task<HttpResponse> Process(string request)
        {
            logger.Info($"REQ {request}");
            if (string.IsNullOrWhiteSpace(request))
            {
                logger.Error("BAD REQ");
                return HttpResponse.BadRequest();
            }

            var parsedRequest = requestParser.Parse(request);

            if (!parsedRequest.Success)
            {
                logger.Error("Could not parse request", parsedRequest);
                return HttpResponse.BadRequest("Could not parse RPC Request");
            }

            var command = commandLocator.Locate(parsedRequest.Request.Method);
            if (command == null)
            {
                logger.Error("Could not locate method", parsedRequest);
                return HttpResponse.NotFound("Could not locate requested method");
            }

            var response = await translator.ExecuteCommand(command, parsedRequest.Request);

            logger.Info("REQ COMPLETE", response);
            return converter.Convert(response);
        }
    }
}