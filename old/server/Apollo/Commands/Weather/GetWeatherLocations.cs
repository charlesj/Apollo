using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Data.Documents;
using Apollo.Services;

namespace Apollo.Commands.Weather
{
    public class GetWeatherLocations : AuthenticatedCommand
    {
        private readonly IWeatherDataService documentStore;

        public GetWeatherLocations(ILoginService loginService, IWeatherDataService documentStore) : base(loginService)
        {
            this.documentStore = documentStore;
        }

        public override async Task<CommandResult> Execute()
        {
            await Task.CompletedTask;
            return CommandResult.CreateSuccessResult(documentStore.GetWeatherLocations().Locations);
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(true);
        }
    }
}
