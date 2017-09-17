using System;
using DarkSky.Models;

namespace Apollo.Data.Documents
{
    public class WeatherForecast : IDocument
    {
        public string Location { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public Forecast Forecast { get; set; }
        public DateTimeOffset LastUpdated { get; set; }

        public string Id => $"forecast:{Location}:{Year}:{Month}:{Day}";
    }
}
