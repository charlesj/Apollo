using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Apollo.Services
{
    public interface IUrlFetcher
    {
        Task<string> Get(string url);
    }

    public class UrlFetcher : IUrlFetcher
    {
        public async Task<string> Get(string url)
        {
            Logger.Trace($"Fetching {url}");
            var request = WebRequest.Create(url);
            var httpResponse = (HttpWebResponse) await request.GetResponseAsync();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                Logger.Trace($"Fetched {url}");
                return streamReader.ReadToEnd();
            }
        }
    }
}
