using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherForecastAPI.Models;

namespace WeatherForecastAPI.Services
{
    public class ConverterService
    {
        public double ConvertetFromCelcius(double temperature, Temperature unit)
        {
            if (unit == Temperature.K) return temperature+273.15;
            else if (unit == Temperature.F) return (temperature * 9 / 5) + 32;
            else if (unit == Temperature.Ra) return (temperature * 273.15)*(9/5);
            else if (unit == Temperature.Re) return temperature * 0.8;
            return temperature;
        }
    }
}