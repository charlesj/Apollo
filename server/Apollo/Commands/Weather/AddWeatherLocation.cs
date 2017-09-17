using System.Linq;
using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Data.Documents;
using Apollo.Services;

namespace Apollo.Commands.Weather
{
    public class AddWeatherLocation : AuthenticatedCommand
    {
        private readonly IApolloDocumentStore documentStore;
        public string Name { get; set; }
        public string Identifier { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public AddWeatherLocation(ILoginService loginService, IApolloDocumentStore documentStore) : base(loginService)
        {
            this.documentStore = documentStore;
        }

        public override async Task<CommandResult> Execute()
        {
            await Task.CompletedTask;
            var location = new WeatherLocation
            {
                Name = Name,
                Identifier = Identifier,
                Latitude = Latitude,
                Longitude = Longitude
            };

            var doc = documentStore.Get<WeatherLocations>(Constants.Documents.WeatherLocations);

            if (doc.Locations.All(l => l.Identifier != Identifier))
            {
                doc.Locations.Add(location);
                documentStore.Upsert(doc);
            }

            return CommandResult.SuccessfulResult;
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(
                !string.IsNullOrWhiteSpace(Name) &&
                !string.IsNullOrWhiteSpace(Identifier));
        }
    }
}
