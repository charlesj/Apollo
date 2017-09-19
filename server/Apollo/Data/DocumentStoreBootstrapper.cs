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
        private readonly IWeatherDataService weatherDocStore;

        public DocumentStoreBootstrapper(IWeatherDataService weatherDocStore)
        {
            this.weatherDocStore = weatherDocStore;
        }

        public Task Bootstrap()
        {
            BootstrapWeatherDoc();

            return Task.CompletedTask;
        }

        private void BootstrapWeatherDoc()
        {
            var current = weatherDocStore.GetWeatherLocations();
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

                weatherDocStore.UpdateLocations(defaultLocations);
                Logger.Info("Created Weather Locations Document");
            }
        }
    }
}
