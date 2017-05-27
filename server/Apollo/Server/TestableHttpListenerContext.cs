using System;
using System.IO;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Text;

namespace Apollo.Server
{
    public class TestableHttpListenerContext : ITestableHttpListenerContext
    {
        private readonly Lazy<HttpListenerContext> lazyContext;

        private HttpListenerContext context => lazyContext.Value;

        [Obsolete("Testing only")]
        public TestableHttpListenerContext()
        {
        }

        public TestableHttpListenerContext(object passedContext)
        {
            this.lazyContext = new Lazy<HttpListenerContext>(() =>
            {
                if (passedContext == null)
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

        public virtual HttpClientInfo GetClientInfo()
        {
            return new HttpClientInfo
            {
                Agent = context.Request.UserAgent,
                IpAddress = context.Request.UserHostAddress
            };
        }
    }
}