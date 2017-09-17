using System.Linq;
using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Data.Documents;
using Apollo.Services;

namespace Apollo.Commands.Weather
{
    public class RemoveWeatherLocation : AuthenticatedCommand
    {
        private readonly IApolloDocumentStore documentStore;
        public string Identifier { get; set; }

        public RemoveWeatherLocation(ILoginService loginService, IApolloDocumentStore documentStore) : base(loginService)
        {
            this.documentStore = documentStore;
        }

        public override async Task<CommandResult> Execute()
        {
            await Task.CompletedTask;
            var doc = documentStore.Get<WeatherLocations>(Constants.Documents.WeatherLocations);
            var location = doc.Locations.FirstOrDefault(l => l.Identifier == Identifier);
            if (location != null)
                doc.Locations.Remove(location);

            documentStore.Upsert(doc);
            return CommandResult.SuccessfulResult;
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(Identifier));
        }
    }
}
