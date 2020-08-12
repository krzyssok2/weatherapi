using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherForecastAPI.Entities;
using WeatherForecastAPI.Models;

namespace WeatherForecastAPI.Services
{
    public class WeatherServices
    {
        WeatherContext _context { get; set; }
        public WeatherServices(WeatherContext context)
        {
            _context = context;
        }
        public double? GetStdev(double? averageActualTemperature, List<Forecasts> forecasts, DateTime date, string provider)
        {

            var mean = averageActualTemperature;

            if (mean == null) return null;

            var sum = forecasts
                        .Where(i => i.ForecastTime.Date == date && i.Provider == provider)
                        .Sum(i => i.Temperature - mean);

            var square = sum * sum;

            var count = forecasts
                .Where(i => i.ForecastTime.Date == date && i.Provider == provider).Count();

            return Math.Sqrt((double)(square / count));
        }
        public List<StdevsProviders> GetProvidersWithStdevs(List<ActualTemperature> actualTemperature, List<Forecasts> forecasts, double? averageActualTemperature)
        {
            var result = forecasts
                .GroupBy(forecast => forecast.Provider)
                .Select(provider => new StdevsProviders
                {
                    ProviderName = provider.Key,
                    Stdevs = forecasts
                    .GroupBy(forecast => forecast.ForecastTime.Date)
                    .Select(Date => new StdevsFactualAndAverage
                    {
                        Date = Date.Key,
                        Factual = actualTemperature
                        .Where(forecast => forecast.ForecastTime.Date == Date.Key)
                        .Average(forecast => (double?)forecast.Temperature),
                        Stdev = GetStdev(averageActualTemperature, forecasts, Date.Key, provider.Key)
                    }).ToList(),
                }).ToList();
            return result;
        }
        public List<Forecasts> GetForecasts(DateTime fromDate, DateTime toDate, long cityId)
        {
            var result = _context.Forecasts
                .Where(f =>
                   f.CitiesId == cityId
                && f.ForecastTime.Date >= fromDate.Date
                && f.ForecastTime.Date <= toDate.Date)
                .ToList();
            return result;
        }
        public List<ActualTemperature> GetActualTemperatures(DateTime fromDate, DateTime toDate, long cityId)
        {
            var result = _context.ActualTemperatures
                .Where(at =>
                   at.CitiesId == cityId
                && at.ForecastTime.Date >= fromDate.Date
                && at.ForecastTime.Date <= toDate.Date)
                .ToList();
            return result;
        }

        public List<CityAverageByDay> GetCityAverageByDay(DateTime fromDate, DateTime toDate, long cityId)
        {
            var result =
                 _context.Forecasts
                .Where(forecastInfo => forecastInfo.CitiesId == cityId
                && forecastInfo.ForecastTime.Date >= fromDate.Date
                && forecastInfo.ForecastTime.Date <= toDate.Date)
                .GroupBy(forecast => forecast.ForecastTime.Date)
                .Select(grouped => new CityAverageByDay
                {
                    Date = grouped.Key,
                    Average = grouped.Average(average => average.Temperature)
                })
                .OrderBy(Average => Average.Date)
                .ToList();

            return result;
        }

        public List<ForecastProvider> GetProvidersWithForecasts(DateTime fromDate, DateTime toDate, long cityId)
        {
            var result =
                _context.Forecasts
                .Where(forecastInfo => forecastInfo.CitiesId == cityId
                && forecastInfo.ForecastTime.Date >= fromDate.Date
                && forecastInfo.ForecastTime.Date <= toDate.Date)
                .ToList()
                .GroupBy(forecast => forecast.Provider)
                .Select(forecasts => new ForecastProvider
                {
                    ProviderName = forecasts.Key,
                    Forcasts = forecasts.Select(forecast => new RawData
                    {
                        Date = forecast.ForecastTime,
                        Temperature = forecast.Temperature
                    })
                    .OrderBy(order => order.Date)
                    .ToList()
                }).ToList();

            return result;
        }

        public List<FactualTemperature> GetActualTemperaturesTransformed(DateTime fromDate, DateTime toDate, long cityId)
        {
            var result =
                _context.ActualTemperatures
                .Where(actualTemperature => actualTemperature.CitiesId == cityId
                && actualTemperature.ForecastTime >= fromDate.Date
                && actualTemperature.ForecastTime <= toDate.Date)
                .Select(actualTemprature => new FactualTemperature
                {
                    Date = actualTemprature.ForecastTime,
                    Temperature = actualTemprature.Temperature
                })
                .OrderBy(temperature => temperature.Date)
                .ToList();

            return result;
        }
    }
}
