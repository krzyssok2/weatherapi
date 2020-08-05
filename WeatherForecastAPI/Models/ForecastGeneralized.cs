using System;
using System.Collections.Generic;

namespace WeatherForecastAPI.Models
{
    public class ForecastGeneralized
    {
        /// <example>vilnius</example>
        public string Name { get; set; }
        /// <example>BBC</example>
        public string Provider { get; set; }
        public DateTime CreationDate { get; set; }
        public List<ForecastsG> Forecasts { get; set; } 
    }
    public class ForecastsG
    {
        public DateTime ForecastTime { get; set; }
        public double temperature { get; set; }
    }

}
