using System;
using System.Threading.Tasks;
using Apollo.Commands;

namespace Apollo.Server
{
    public interface IJsonRPCProcessor
    {
        Task<HttpResponse> Process(string request);
    }

    public class JsonRPCProcessor : IJsonRPCProcessor
    {
        private readonly ICommandLocator commandLocator;
        private readonly IJsonRPCRequestParser requestParser;
        private readonly IJsonRPCCommandTranslator translator;
        private readonly IJsonRPCHttpConverter converter;

        public JsonRPCProcessor(
            ICommandLocator commandLocator,
            IJsonRPCCommandTranslator translator,
            IJsonRPCHttpConverter converter,
            IJsonRPCRequestParser requestParser)
        {
            this.commandLocator = commandLocator;
            this.requestParser = requestParser;
            this.translator = translator;
            this.converter = converter;
        }

        public async Task<HttpResponse> Process(string request)
        {
            if (string.IsNullOrWhiteSpace(request))
                return HttpResponse.BadRequest();

            var parsedRequest = requestParser.Parse(request);

            if (!parsedRequest.Success)
                return HttpResponse.BadRequest("Could not parse RPC Request");

            var command = commandLocator.Locate(parsedRequest.Request.Method);
            if (command == null)
                return HttpResponse.NotFound("Could not locate requested method");

            var response = await translator.ExecuteCommand(command, parsedRequest.Request);

            return converter.Convert(response);
        }
    }
}