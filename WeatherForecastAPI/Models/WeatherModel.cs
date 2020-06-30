using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherForecastAPI.Models
{
    public class WeatherModel
    {
    }
    public class WeatherRawForecasts
    {
        public long CityId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public List<ForecastProvider> ForecastsProvider{ get;set; }
    }
    public class ForecastProvider
    {
        public string Provider { get; set; }
        List<RawData> Forcasts { get; set; }
    }
    public class RawData
    {
        public string Date { get; set; }
        public double Temperature { get; set; }
    }
    public class WeatherCityAverage
    {
        public long CityId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public List<CityAverageByDay> CityAverageByDay { get; set; }
    }
    public class CityAverageByDay
    {
        public string Date { get; set; }
        public double Average { get; set; }
    }
    public class AllStdevs
    {
        public string CityId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public List<StdevsProviders> StdevsProviders { get; set; }
    }
    public class StdevsProviders
    {
        public string Provider { get; set; }
        public List<StdevsFactualAndAverage> StdevsDataByDay { get; set; }
    }
    public class StdevsFactualAndAverage
    {
        public string Date { get; set; }
        public double Factual { get; set; }
        public double Stdev { get; set; }
    }
}
