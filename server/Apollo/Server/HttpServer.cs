using System;
using System.Net;
using System.Threading;

namespace Apollo.Server
{
    public class HttpServer : IHttpServer
    {
        private readonly IJsonRpcProcessor processor;
        private string prefix = "http://localhost:8042/";
        private HttpListener listener;

        public HttpServer(IJsonRpcProcessor processor)
        {
            this.processor = processor;
            if (!HttpListener.IsSupported)
            {
                throw new Exception("HTTPListener is not supported on this system.");
            }

            listener = new HttpListener();
            listener.Prefixes.Add(prefix);
        }

        public void Listen()
        {
            listener.Start();
            Console.WriteLine($"Server listening on {prefix}");
            ThreadPool.QueueUserWorkItem((o) =>
            {
                while (listener.IsListening)
                {
                    ThreadPool.QueueUserWorkItem(c => ProcessRequest(new TestableHttpListenerContext(c)), listener.GetContext());
                }
            });
        }

        public void ProcessRequest(ITestableHttpListenerContext context)
        {
            try
            {
                context.AddHeader("Access-Control-Allow-Origin", "*");
                context.AddHeader("Access-Control-Allow-Headers", "X-Requested-With, X-HTTP-Method-Override, Content-Type, Accept");
                context.AddHeader("Access-Control-Allow-Methods", "POST");
                context.AddHeader("X-Powered-By", $"Apollo v{Apollo.Version}");

                if (context.HttpMethod == "OPTIONS" || context.HttpMethod == "HEAD")
                {
                    context.CloseResponse();
                    return;
                }

                var body = context.GetRequestBody();
                var response = processor.Process(body).GetAwaiter().GetResult();

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