using System;
using System.Collections.Generic;
namespace WeatherForecastAPI.Models
{
    public class WeatherRawForecasts
    {
        /// <example>1</example>
        public long CityId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<FactualTemperature> FactualTemperature { get;set;}
        public List<ForecastProvider> Providers{ get;set; }
    }
    public class FactualTemperature
    {
        public DateTime Date { get; set; }
        /// <example>25</example>
        public double Temperature { get; set; }
    }
    public class ForecastProvider
    {
        /// <example>BBC</example>
        public string ProviderName { get; set; }
        public List<RawData> Forcasts { get; set; }
    }
    public class RawData
    {
        public DateTime Date { get; set; }
        /// <example>25</example>
        public double Temperature { get; set; }
    }
    public class WeatherCityAverage
    {
        /// <example>1</example>
        public long CityId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<CityAverageByDay> Average { get; set; }
    }
    public class CityAverageByDay
    {
        public DateTime Date { get; set; }
        /// <example>25</example>
        public double Average { get; set; }
    }
    public class AllStdevs
    {
        /// <example>1</example>
        public long CityId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<StdevsProviders> Providers { get; set; }
    }
    public class StdevsProviders
    {
        /// <example>BBC</example>
        public string ProviderName { get; set; }
        public List<StdevsFactualAndAverage> Stdevs { get; set; }
    }
    public class StdevsFactualAndAverage
    {
        public DateTime Date { get; set; }
        /// <example>18</example>
        public double? Factual { get; set; }
        /// <example>3</example>
        public double? Stdev { get; set; }
    }
}
