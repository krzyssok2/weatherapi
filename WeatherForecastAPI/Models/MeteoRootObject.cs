using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherForecastAPI.Models
{
    public class MeteoRootObject
    {
        public Place place { get; set; }
        public string forecastType {get;set;}
        public string forecastCreationTimeUtc { get; set; }
        public List<ForecastTimestamps> forecastTimestamps { get; set; }
    }
    public class Place
    {
        public string code { get; set; }
        public string name { get; set; }
        public string administrativeDivision { get; set; }
        public string country { get; set; }
        public string countryCode { get; set; }
        public Coordinates coordinates { get; set; }

    }
    public class Coordinates
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }
    public class ForecastTimestamps
    {
        public string forecastTimeUtc { get; set; }
        public double airTemperature { get; set; }
        public double windSpeed { get; set; }
        public double windGust { get; set; }
        public double windDirection { get; set; }
        public double cloudCover { get; set; }
        public double seaLevelPressure { get; set; }
        public double relativeHumidity { get; set; }
        public double totalPrecipitation { get; set; }
        public string conditionCode { get; set; }
    }
}