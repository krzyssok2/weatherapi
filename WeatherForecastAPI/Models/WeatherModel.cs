using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherForecastAPI.Models
{
    public class WeatherModel
    {
    }
    public class WeatherCityAverage
    {
        public string CityName { get; set; }
        public string Country { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public double Average { get; set; }
    }
    public class WeatherAllAverages
    {
        public List<WeatherCityAverage> AllCitiesAverage { get; set; }
    }
    public class AllStdevs
    {
        public List<Stdevs> allstevs { get; set; }
    }
    public class Stdevs
    {
        public string Provider { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public List<StdevsFactualAndAverage> StdevsDataByDay { get; set; }
    }
    public class StdevsFactualAndAverage
    {
        public string Date { get; set; }
        public double FactualTemperature { get; set; }
        public double StdevTemperature { get; set; }
    }
}
