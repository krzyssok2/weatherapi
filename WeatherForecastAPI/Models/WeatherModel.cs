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
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<FactualTemperature> FactualTemperature { get;set;}
        public List<ForecastProvider> Providers{ get;set; }
    }
    public class FactualTemperature
    {
        public DateTime Date { get; set; }
        public double Temperature { get; set; }
    }
    public class ForecastProvider
    {
        public string ProviderName { get; set; }
        public List<RawData> Forcasts { get; set; }
    }
    public class RawData
    {
        public DateTime Date { get; set; }
        public double Temperature { get; set; }
    }
    public class WeatherCityAverage
    {
        public long CityId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<CityAverageByDay> Average { get; set; }
    }
    public class CityAverageByDay
    {
        public DateTime Date { get; set; }
        public double Average { get; set; }
    }
    public class AllStdevs
    {
        public long CityId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<StdevsProviders> Providers { get; set; }
    }
    public class StdevsProviders
    {
        public string ProviderName { get; set; }
        public List<StdevsFactualAndAverage> Stdevs { get; set; }
    }
    public class StdevsFactualAndAverage
    {
        public DateTime Date { get; set; }
        public double Factual { get; set; }
        public double Stdev { get; set; }
    }
}
