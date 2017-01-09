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
                    ThreadPool.QueueUserWorkItem((ctx) =>
                    {
                        var context = (HttpListenerContext) ctx;
                        context.Response.AddHeader("Access-Control-Allow-Origin", "*");
                        context.Response.AddHeader("Access-Control-Allow-Headers", "X-Requested-With, X-HTTP-Method-Override, Content-Type, Accept");
                        context.Response.AddHeader("Access-Control-Allow-Methods", "POST");
                        try
                        {
                            if (context.Request.HttpMethod == "OPTIONS" || context.Request.HttpMethod == "HEAD")
                            {
                                context.Response.Close();
                                return;
                            }

                            var body = new StreamReader(context.Request.InputStream).ReadToEnd();
                            var response = processor.Process(body).GetAwaiter().GetResult();
                            var bodyBytes = Encoding.UTF8.GetBytes(response.Body);
                            context.Response.StatusCode = response.HttpCode;

                            context.Response.KeepAlive = false;
                            context.Response.ContentLength64 = bodyBytes.Length;
                            var output = context.Response.OutputStream;
                            output.Write(bodyBytes, 0, bodyBytes.Length);
                            context.Response.Close();
                        }
                        catch (Exception exception)
                        {
                            context.Response.StatusCode = 503;
                            var output = context.Response.OutputStream;
                            var bodyBytes = Encoding.UTF8.GetBytes($"Eh?  I can't understand you: {exception.Message}");
                            output.Write(bodyBytes, 0, bodyBytes.Length);
                            context.Response.Close();
                        }
                    }, listener.GetContext());
                }
            });
        }
    }
}