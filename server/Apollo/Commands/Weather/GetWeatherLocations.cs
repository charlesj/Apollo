using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Data.Documents;
using Apollo.Services;

namespace Apollo.Commands.Weather
{
    public class GetWeatherLocations : AuthenticatedCommand
    {
        private readonly IApolloDocumentStore documentStore;

        public GetWeatherLocations(ILoginService loginService, IApolloDocumentStore documentStore) : base(loginService)
        {
            this.documentStore = documentStore;
        }

        public override async Task<CommandResult> Execute()
        {
            await Task.CompletedTask;
            var document = documentStore.Get<WeatherLocations>(Constants.Documents.WeatherLocations);
            return CommandResult.CreateSuccessResult(document.Locations);
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(true);
        }
    }
}
