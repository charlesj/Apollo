using System.Threading.Tasks;
using DarkSky.Models;
using DarkSky.Services;

namespace Apollo.External.DarkSky
{
    public class DarkSkyWeatherService
    {
        public virtual async Task<Forecast> GetForecast(string apiKey, double latitude, double longitude)
        {
            var service = new DarkSkyService(apiKey);
            var response = await service.GetForecast(latitude, longitude);
            if (!response.IsSuccessStatus)
            {
                Logger.Error("Failed to talk to dark sky", response);
            }

            return response.Response;
        }
    }
}
