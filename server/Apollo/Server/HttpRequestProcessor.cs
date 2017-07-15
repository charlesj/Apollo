using System;
using System.Threading.Tasks;

namespace Apollo.Server
{
    public class HttpRequestProcessor : IHttpRequestProcessor
    {
        private readonly IJsonRpcProcessor processor;

        public HttpRequestProcessor(IJsonRpcProcessor processor)
        {
            this.processor = processor;
        }

        public async Task Process(ITestableHttpContext context)
        {
            try
            {
                Logger.Trace("Receiving Request");
                context.AddHeader("X-Powered-By", $"Apollo v{Apollo.Version}");

                if (context.HttpMethod == "OPTIONS" || context.HttpMethod == "HEAD")
                {
                    context.CloseResponse();
                    return;
                }

                Logger.Trace("Processing Request");

                var body = context.GetRequestBody();
                var clientInfo = context.GetClientInfo();
                var response = await processor.Process(body, clientInfo);

                context.StatusCode = response.HttpCode;

                context.WriteResponseBody(response.Body);

                context.CloseResponse();
            }
            catch (Exception exception)
            {
                context.StatusCode = 503;
                context.WriteResponseBody($"SERVER ERROR: {exception.Message}");
                context.CloseResponse();
            }
        }
    }
}
