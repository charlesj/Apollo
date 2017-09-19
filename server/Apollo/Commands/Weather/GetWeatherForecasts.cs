using System.Linq;
using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Weather
{
    public class GetWeatherForecasts : AuthenticatedCommand
    {
        private readonly IWeatherDataService dataService;

        public GetWeatherForecasts(ILoginService loginService, IWeatherDataService dataService) : base(loginService)
        {
            this.dataService = dataService;
        }

        public override async Task<CommandResult> Execute()
        {
            await Task.CompletedTask;

            var locations = dataService.GetWeatherLocations();
            Logger.Trace("Got Locations", locations);
            var forecasts = locations.Locations.Select(l => dataService.GetMostRecentForecast(l.Identifier)).ToList();
            Logger.Trace("Forcasts", forecasts);

            return CommandResult.CreateSuccessResult(new {locations, forecasts});
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(true);
        }
    }
}
