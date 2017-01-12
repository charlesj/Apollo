using System;
using System.IO;
using System.Net;
using System.Text;
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

        public interface ITestableHttpListenerContext
        {
            string HttpMethod { get; }
            int StatusCode { get; set; }
            string GetRequestBody();
            void WriteResponseBody(string body);
            void AddHeader(string headerName, string value);
            void CloseResponse();
        }

        public class TestableHttpListenerContext : ITestableHttpListenerContext
        {
            private Lazy<HttpListenerContext> lazyContext;

            private HttpListenerContext context
            {
                get { return lazyContext.Value; }
                set {}
            }

            [Obsolete("Testing only")]
            public TestableHttpListenerContext(){}

            public TestableHttpListenerContext(object passedContext)
            {
                this.lazyContext = new Lazy<HttpListenerContext>(() =>
                {
                    if(passedContext == null)
                        throw new ArgumentNullException(
                            nameof(passedContext),
                            "Null HttpListenerContext Encountered at Lazy Evaluation");
                    var castContext = (HttpListenerContext) passedContext;
                    castContext.Response.KeepAlive = false;
                    return castContext;
                });
            }

            public virtual string HttpMethod => context.Request.HttpMethod;

            public virtual int StatusCode
            {
                get { return context.Response.StatusCode; }
                set { context.Response.StatusCode = value; }
            }

            public virtual string GetRequestBody()
            {
                return new StreamReader(context.Request.InputStream).ReadToEnd();
            }

            public void WriteResponseBody(string body)
            {
                var bodyBytes = Encoding.UTF8.GetBytes(body);
                context.Response.ContentLength64 = bodyBytes.Length;
                var output = context.Response.OutputStream;
                output.Write(bodyBytes, 0, bodyBytes.Length);
            }

            public virtual void AddHeader(string headerName, string value)
            {
                context.Response.AddHeader(headerName, value);
            }

            public virtual void CloseResponse()
            {
                context.Response.Close();
            }
        }
    }
}