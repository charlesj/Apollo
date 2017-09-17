using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Data.Documents;
using Apollo.External.DarkSky;
using Apollo.Services;
using Apollo.Utilities;

namespace Apollo.Commands.Weather
{
    public class UpdateWeather : AuthenticatedCommand
    {
        private readonly IApolloDocumentStore documentStore;
        private readonly IClock clock;
        private readonly DarkSkyWeatherService weatherService;
        public string ApiKey { get; set; }

        public UpdateWeather(
            ILoginService loginService,
            IApolloDocumentStore documentStore,
            IClock clock,
            DarkSkyWeatherService weatherService) : base(loginService)
        {
            this.documentStore = documentStore;
            this.clock = clock;
            this.weatherService = weatherService;
        }

        public override async Task<CommandResult> Execute()
        {
            Logger.Trace("Getting tracked locations");
            var doc = documentStore.Get<WeatherLocations>(Constants.Documents.WeatherLocations);
            if (doc == null)
            {
                throw new InvalidOperationException("Cannot get weather locations");
            }
            Logger.Trace("Got Locations");
            var now = clock.UtcNow;
            var results = new List<string>();
            foreach (var location in doc.Locations)
            {
                Logger.Trace($"Checking forecast", location);
                var forecast = await weatherService.GetForecast(ApiKey, location.Latitude, location.Longitude);
                var foreDoc = new WeatherForecast
                {
                    Location = location.Identifier,
                    Year = now.Year,
                    Month = now.Month,
                    Day = now.Day,
                    Forecast = forecast,
                    LastUpdated = now
                };

                Logger.Trace("Built Forecast", foreDoc);

                documentStore.Upsert(foreDoc);
                results.Add($"Upserted {foreDoc.Id}");
            }

            return CommandResult.CreateSuccessResult(results);
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(ApiKey));
        }
    }
}
