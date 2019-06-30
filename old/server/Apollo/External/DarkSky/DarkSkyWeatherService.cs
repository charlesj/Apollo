using System.Threading.Tasks;
using Apollo.Services;
using Newtonsoft.Json.Linq;

namespace Apollo.External.DarkSky
{
    public class DarkSkyWeatherService
    {
        private readonly IUrlFetcher urlFetcher;

        public DarkSkyWeatherService(IUrlFetcher urlFetcher)
        {
            this.urlFetcher = urlFetcher;
        }

        public virtual async Task<JObject> GetForecast(string apiKey, double latitude, double longitude)
        {
            var url = $"https://api.darksky.net/forecast/{apiKey}/{latitude},{longitude}";
            var result = await urlFetcher.Get(url);
            return JObject.Parse(result);
        }
    }
}
