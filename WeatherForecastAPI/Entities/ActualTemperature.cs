using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherForecastAPI.Entities
{
    public class ActualTemperature
    {
        public long Id { get; set; }
        public DateTime ForecastTime { get; set; }
        public double Temperature { get; set; }
        public long CitiesId { get; set; }
    }
}
