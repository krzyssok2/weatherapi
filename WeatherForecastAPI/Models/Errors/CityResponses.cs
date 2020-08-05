using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherForecastAPI.Models.Swagger
{
    public enum CityErrors
    {
       NoDataFound
    }
    public class CityErrorsResponse
    {
        public CityErrors Error { get; set; }
    }
}
