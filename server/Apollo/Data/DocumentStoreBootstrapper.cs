using System.Collections.Generic;
using System.Threading.Tasks;
using Apollo.Data.Documents;

namespace Apollo.Data
{
    public interface IDocumentStoreBoostrapper
    {
        Task Bootstrap();
    }

    public class DocumentStoreBootstrapper : IDocumentStoreBoostrapper
    {
        private readonly IApolloDocumentStore documentStore;

        public DocumentStoreBootstrapper(IApolloDocumentStore documentStore)
        {
            this.documentStore = documentStore;
        }

        public Task Bootstrap()
        {
            BootstrapWeatherDoc();

            return Task.CompletedTask;
        }

        private void BootstrapWeatherDoc()
        {
            var current = documentStore.Get<WeatherLocations>(Constants.Documents.WeatherLocations);
            if (current == null)
            {
                var defaultLocations = new WeatherLocations
                {
                    Locations = new List<WeatherLocation>
                    {
                        new WeatherLocation
                        {
                            Name = "Bothell, WA",
                            Identifier = "home",
                            Latitude = 47.7610,
                            Longitude = 122.2056
                        }
                    }
                };

                documentStore.Upsert(defaultLocations);
                Logger.Info("Created Weather Locations Document");
            }
        }
    }
}
