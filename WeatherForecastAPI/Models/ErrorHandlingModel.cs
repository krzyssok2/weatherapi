using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherForecastAPI.Models
{
    public class ErrorHandlingModel
    {
        public int Error { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
    }
}
