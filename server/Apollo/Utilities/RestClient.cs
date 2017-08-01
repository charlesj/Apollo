using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Apollo.Utilities
{
    public interface IRestClient
    {
        Task<TRestModel> GetJson<TRestModel>(string url);
    }

    public class RestClient : IRestClient
    {
        private readonly IJsonSerializer serializer;
        private HttpClient client;

        public RestClient(IJsonSerializer serializer)
        {
            this.serializer = serializer;
            this.client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("User-Agent", $"Apollo Server/{Apollo.Version}");
        }

        public async Task<TRestModel> GetJson<TRestModel>(string url)
        {
            try
            {
                var response = await client.GetAsync(url);
                var body = await response.Content.ReadAsStringAsync();
                Logger.Trace("Rest Request", new {url, body});
                if (response.IsSuccessStatusCode)
                {
                    return serializer.Deserialize<TRestModel>(body);
                }

                Logger.Error($"Error getting json from {url}", new {response, body});
                throw new RestClientException($"Error getting json from {url}"){ Body = body};
            }
            catch (Exception e)
            {
                Logger.Error($"Error getting json from {url}: {e.Message} ({e.GetType()}");
                throw;
            }
        }
    }

    public class RestClientException : Exception
    {
        public RestClientException(string message) : base(message)
        {
        }

        public string Body { get; set; }
    }
}
