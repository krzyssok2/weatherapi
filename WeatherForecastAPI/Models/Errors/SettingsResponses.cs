using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherForecastAPI.Models.Swagger
{
    public enum SettingsErrors
    {
        CityNotFound
    }

    public class SettingsErrorResponse
    {
        public SettingsErrors Error { get; set; }
    }
}
