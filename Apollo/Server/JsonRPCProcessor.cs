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

        public JsonRPCProcessor(
            ICommandLocator commandLocator,
            IJsonRPCRequestParser requestParser)
        {
            this.commandLocator = commandLocator;
            this.requestParser = requestParser;
        }

        public async Task<HttpResponse> Process(string request)
        {
            await Task.FromResult(0);
            if (string.IsNullOrWhiteSpace(request))
                return HttpResponse.BadRequest();

            var parsedRequest = requestParser.Parse(request);

            if (!parsedRequest.Success)
                return HttpResponse.BadRequest("Could not parse RPC Request");

            var command = commandLocator.Locate(parsedRequest.Request.Method);
            if (command == null)
                return HttpResponse.NotFound("Could not locate requested method");

            return new HttpResponse {Body = $"processed at {DateTime.Now}", HttpCode = 200};
        }
    }
}