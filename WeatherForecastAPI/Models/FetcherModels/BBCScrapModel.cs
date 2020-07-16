using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherForecastAPI.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject(myJsonResponse); 
    public class Options
    {
        public string location_id { get; set; }
        public string day { get; set; }
        public string locale { get; set; }

    }

    public class Report
    {
        //public string enhancedWeatherDescription { get; set; }
        //public int extendedWeatherType { get; set; }
        //public int feelsLikeTemperatureC { get; set; }
        //public int feelsLikeTemperatureF { get; set; }
        //public int gustSpeedKph { get; set; }
        //public int gustSpeedMph { get; set; }
        //public int humidity { get; set; }
        public string localDate { get; set; }
        //public int precipitationProbabilityInPercent { get; set; }
        //public string precipitationProbabilityText { get; set; }
        //public int pressure { get; set; }
        public int temperatureC { get; set; }
        //public int temperatureF { get; set; }
        public string timeslot { get; set; }
        public int timeslotLength { get; set; }
        //public string visibility { get; set; }
        //public int weatherType { get; set; }
        //public string weatherTypeText { get; set; }
        //public string windDescription { get; set; }
        //public string windDirection { get; set; }
        //public string windDirectionAbbreviation { get; set; }
        //public string windDirectionFull { get; set; }
        //public int windSpeedKph { get; set; }
        //public int windSpeedMph { get; set; }

    }

    public class Detailed
    {
        public DateTime issueDate { get; set; }
        public DateTime lastUpdated { get; set; }
        public List<Report> reports { get; set; }

    }

    public class Report2
    {
        public string enhancedWeatherDescription { get; set; }
        public int gustSpeedKph { get; set; }
        public int gustSpeedMph { get; set; }
        public string localDate { get; set; }
        public object lowermaxTemperatureC { get; set; }
        public object lowermaxTemperatureF { get; set; }
        public object lowerminTemperatureC { get; set; }
        public object lowerminTemperatureF { get; set; }
        public int maxTempC { get; set; }
        public int maxTempF { get; set; }
        public int minTempC { get; set; }
        public int minTempF { get; set; }
        public int mostLikelyHighTemperatureC { get; set; }
        public int mostLikelyHighTemperatureF { get; set; }
        public int mostLikelyLowTemperatureC { get; set; }
        public int mostLikelyLowTemperatureF { get; set; }
        public object pollenIndex { get; set; }
        public object pollenIndexBand { get; set; }
        public object pollenIndexIconText { get; set; }
        public object pollenIndexText { get; set; }
        public object pollutionIndex { get; set; }
        public object pollutionIndexBand { get; set; }
        public object pollutionIndexIconText { get; set; }
        public object pollutionIndexText { get; set; }
        public int precipitationProbabilityInPercent { get; set; }
        public string precipitationProbabilityText { get; set; }
        public string sunrise { get; set; }
        public string sunset { get; set; }
        public object uppermaxTemperatureC { get; set; }
        public object uppermaxTemperatureF { get; set; }
        public object upperminTemperatureC { get; set; }
        public object upperminTemperatureF { get; set; }
        public int uvIndex { get; set; }
        public string uvIndexBand { get; set; }
        public string uvIndexIconText { get; set; }
        public string uvIndexText { get; set; }
        public int weatherType { get; set; }
        public string weatherTypeText { get; set; }
        public string windDescription { get; set; }
        public string windDirection { get; set; }
        public string windDirectionAbbreviation { get; set; }
        public string windDirectionFull { get; set; }
        public int windSpeedKph { get; set; }
        public int windSpeedMph { get; set; }

    }

    public class Summary
    {
        public DateTime issueDate { get; set; }
        public DateTime lastUpdated { get; set; }
        public Report2 report { get; set; }

    }

    public class ForecastOWN
    {
        public Detailed detailed { get; set; }
        //public Summary summary { get; set; }

    }

    public class Location
    {
        public string id { get; set; }
        public string name { get; set; }
        public string container { get; set; }
        //public double latitude { get; set; }
        //public double longitude { get; set; }

    }

    public class Data
    {
        public List<ForecastOWN> forecasts { get; set; }
        //public bool isNight { get; set; }
        //public DateTime issueDate { get; set; }
        //public DateTime lastUpdated { get; set; }
        public Location location { get; set; }
        //public bool night { get; set; }

    }

    public class FeatureFlags
    {
        public bool useAlgorithmicText { get; set; }

    }

    public class BBCScrapRootObject
    {
        //public Options options { get; set; }
        public Data data { get; set; }
        //public string environment { get; set; }
        //public string locatorKey { get; set; }
        //public string uasKey { get; set; }
        //public string weatherApiUri { get; set; }
        //public FeatureFlags featureFlags { get; set; }

    }


}
