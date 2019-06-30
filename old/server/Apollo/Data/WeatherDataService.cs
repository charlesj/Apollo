using System.Linq;
using Apollo.Data.Documents;
using Apollo.Utilities;
using Newtonsoft.Json.Linq;

namespace Apollo.Data
{
    public interface IWeatherDataService
    {
        WeatherLocations GetWeatherLocations();
        void UpdateLocations(WeatherLocations defaultLocations);
        void UpdateForecast(WeatherLocation location, JObject forecast);
        WeatherForecast GetMostRecentForecast(string location);
    }

    public class WeatherDataService : ApolloDocumentStore, IWeatherDataService
    {
        private readonly IClock clock;

        public WeatherDataService(IClock clock, IConfiguration configuration) : base(configuration)
        {
            this.clock = clock;
        }

        public WeatherLocations GetWeatherLocations()
        {
            return Get<WeatherLocations>(Constants.Documents.WeatherLocations);
        }

        public void UpdateLocations(WeatherLocations defaultLocations)
        {
            Upsert(defaultLocations);
        }

        public void UpdateForecast(WeatherLocation location, JObject forecast)
        {
            var now = clock.UtcNow;
            var foreDoc = new WeatherForecast
            {
                Location = location.Identifier,
                Year = now.Year,
                Month = now.Month,
                Day = now.Day,
                Forecast = forecast,
                LastUpdated = now
            };
            Logger.Trace("forecast doc", foreDoc);
            Upsert(foreDoc);
        }

        public WeatherForecast GetMostRecentForecast(string location)
        {
            using (var session = documentStore.LightweightSession())
            {
                return session.Query<WeatherForecast>()
                    .Where(f => f.Location == location)
                    .OrderByDescending(f => f.LastUpdated)
                    .FirstOrDefault();
            }
        }
    }
}
