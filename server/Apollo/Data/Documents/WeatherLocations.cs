using System.Collections.Generic;

namespace Apollo.Data.Documents
{
    public class WeatherLocations : IDocument
    {
        public string Id => Constants.Documents.WeatherLocations;
        public List<WeatherLocation> Locations { get; set; }
    }

    public class WeatherLocation
    {
        public string Name { get; set; }
        public string Identifier { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
