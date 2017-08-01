using System;
using System.Net.Http;
using System.Threading.Tasks;
using Apollo.Utilities;

namespace Apollo.External.Coinbase
{
    public interface IGdaxClient
    {
        Task<ProductTicker> GetProductTicker(string productId);
    }

    public class GdaxClient : IGdaxClient
    {
        private readonly IRestClient client;


        public GdaxClient(IRestClient client)
        {
            this.client = client;
        }

        public Task<ProductTicker> GetProductTicker(string productId)
        {
            return client.GetJson<ProductTicker>($"https://api.gdax.com/products/{productId}/ticker");
        }
    }

    public class ProductTicker
    {
        public string Price { get; set; }
    }
}
