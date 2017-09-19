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
        private readonly IWeatherDataService documentStore;
        public string Identifier { get; set; }

        public RemoveWeatherLocation(ILoginService loginService, IWeatherDataService documentStore) : base(loginService)
        {
            this.documentStore = documentStore;
        }

        public override async Task<CommandResult> Execute()
        {
            await Task.CompletedTask;
            var doc = documentStore.GetWeatherLocations();
            var location = doc.Locations.FirstOrDefault(l => l.Identifier == Identifier);
            if (location != null)
                doc.Locations.Remove(location);

            documentStore.UpdateLocations(doc);
            return CommandResult.SuccessfulResult;
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(Identifier));
        }
    }
}
