using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherForecastAPI.Models.Swagger
{
    public enum ErrorsWeather
    {
        TimeSpanTooLong,
        DataNotFound,
    }
    public class WeatherErrorsResponse
    {
        public ErrorsWeather Error { get; set; }
    }
}
