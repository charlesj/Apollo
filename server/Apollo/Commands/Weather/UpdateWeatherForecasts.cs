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
    public class UpdateWeatherForecasts : AuthenticatedCommand
    {
        private readonly IWeatherDataService documentStore;
        private readonly IClock clock;
        private readonly DarkSkyWeatherService weatherService;
        public string ApiKey { get; set; }

        public UpdateWeatherForecasts(
            ILoginService loginService,
            IWeatherDataService documentStore,
            IClock clock,
            DarkSkyWeatherService weatherService) : base(loginService)
        {
            this.documentStore = documentStore;
            this.clock = clock;
            this.weatherService = weatherService;
        }

        public override async Task<CommandResult> Execute()
        {
            var doc = documentStore.GetWeatherLocations();

            var results = new List<string>();
            foreach (var location in doc.Locations)
            {
                var forecast = await weatherService.GetForecast(ApiKey, location.Latitude, location.Longitude);
                Logger.Trace("got forecast", forecast);
                documentStore.UpdateForecast(location, forecast);
                results.Add($"Upserted {location.Identifier}");
            }

            return CommandResult.CreateSuccessResult(results);
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(ApiKey));
        }
    }
}
