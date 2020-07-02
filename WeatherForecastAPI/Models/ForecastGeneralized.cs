using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherForecastAPI.Models
{
    public class ForecastGeneralized
    {
        public string Name { get; set; }
        public string Provider { get; set; }
        public DateTime CreationDate { get; set; }
        public List<Forecasts> Forecasts { get; set; } 
    }
    public class Forecasts
    {
        public DateTime ForecastTime { get; set; }
        public double temperature { get; set; }
    }

}
