using System.Threading.Tasks;

namespace Apollo.Server
{
    public interface IHttpRequestProcessor
    {
        Task Process(ITestableHttpContext context);
    }
}
