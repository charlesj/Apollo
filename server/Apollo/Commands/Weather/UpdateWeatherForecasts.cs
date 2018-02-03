using System.Collections.Generic;
using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.External.DarkSky;
using Apollo.Services;
using Apollo.Utilities;

namespace Apollo.Commands.Weather
{
    public class UpdateWeatherForecasts : AuthenticatedCommand
    {
        private readonly IWeatherDataService documentStore;
        private readonly IClock clock;
        private readonly IUserSettignsDataService userSettingsDataService;
        private readonly DarkSkyWeatherService weatherService;

        public UpdateWeatherForecasts(
            ILoginService loginService,
            IWeatherDataService documentStore,
            IClock clock,
            IUserSettignsDataService userSettingsDataService,
            DarkSkyWeatherService weatherService) : base(loginService)
        {
            this.documentStore = documentStore;
            this.clock = clock;
            this.userSettingsDataService = userSettingsDataService;
            this.weatherService = weatherService;
        }

        public override async Task<CommandResult> Execute()
        {
            var doc = documentStore.GetWeatherLocations();
            var apiKeySetting = await userSettingsDataService.GetUserSetting(Constants.UserSettings.DarkSkyApiKey);
            var results = new List<string>();
            foreach (var location in doc.Locations)
            {
                var forecast = await weatherService.GetForecast(apiKeySetting.value, location.Latitude, location.Longitude);
                Logger.Trace("got forecast", forecast);
                documentStore.UpdateForecast(location, forecast);
                results.Add($"Upserted {location.Identifier}");
            }

            return CommandResult.CreateSuccessResult(results);
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(true);
        }
    }
}
