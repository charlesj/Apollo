using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Apollo.Server
{
    public class TestableHttpContext : ITestableHttpContext
    {
        private HttpContext context;

        [Obsolete("Testing only")]
        public TestableHttpContext()
        {
        }

        public TestableHttpContext(HttpContext context)
        {
            this.context = context;
        }

        public virtual string HttpMethod => context.Request.Method;

        public virtual int StatusCode
        {
            get { return context.Response.StatusCode; }
            set { context.Response.StatusCode = value; }
        }

        public virtual string GetRequestBody()
        {
            return new StreamReader(context.Request.Body).ReadToEnd();
        }

        public void WriteResponseBody(string body)
        {
            var bodyBytes = Encoding.UTF8.GetBytes(body);
            context.Response.ContentLength = bodyBytes.Length;
            var output = context.Response.Body;
            output.Write(bodyBytes, 0, bodyBytes.Length);
        }

        public virtual void AddHeader(string headerName, string value)
        {
            Logger.Trace($"Adding header {headerName} with value {value}");
            context.Response.Headers.Add(headerName, new[] { value });
        }

        public virtual void CloseResponse()
        {
            context.Response.Clear();
        }

        public virtual HttpClientInfo GetClientInfo()
        {
            var originAddress = context.Request.Headers["X-Forwarded-For"];

            return new HttpClientInfo
            {
                Agent = context.Request.Headers["User-Agent"].ToString(),
                IpAddress = originAddress == StringValues.Empty ? originAddress.First() : context.Connection.RemoteIpAddress.ToString()
            };
        }
    }
}
