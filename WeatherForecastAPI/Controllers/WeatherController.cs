using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WeatherForecastAPI.Models;
using WeatherForecastAPI.Entities;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace WeatherForecastAPI.Controllers
{
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private WeatherContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        public WeatherController(WeatherContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }


        /// <summary>
        /// Get stdev of the city
        /// </summary>
        [HttpGet("stdev/{CityId}")]
        public ActionResult<AllStdevs> GetAllStdevsFrom(long cityId, DateTime fromDate, DateTime toDate)
        {
            if (fromDate == default)
            {
                fromDate = DateTime.Now.Date;
            }

            if (toDate == default)
            {
                toDate = DateTime.Now.Date;
            }

            if ((toDate - fromDate).TotalDays > 14 || (toDate - fromDate).TotalDays < 0)
            {
                Log.Error($"No data was provided for city with Id {cityId} as time period was too big {fromDate.Date} - {toDate.Date}");
                return BadRequest(new ErrorHandlingModel
                {
                    Error = 400,
                    Message = "Bad Request",
                    Description = "Time between days can't be negative or be higher than 14"
                });
            }
            var forecasts = GetForecasts(fromDate, toDate, cityId);
            var actualTemperature = GetActualTemperatures(fromDate, toDate, cityId);
            var averageActualTemperature = actualTemperature.Average(i =>(double?) i.Temperature);
            var providers = GetProvidersWithStdevs(fromDate, toDate, cityId, actualTemperature, forecasts, averageActualTemperature);

            AllStdevs allStdevs = new AllStdevs
            {
                CityId = cityId,
                FromDate = fromDate,
                ToDate = toDate,
                Providers= providers
            };
            foreach(var provider in allStdevs.Providers.ToList())
            {
                foreach (var stdev in provider.Stdevs.ToList())
                {
                    if (stdev.Factual == null)
                    {
                        provider.Stdevs.Remove(stdev);
                    }
                }
            }
            if (allStdevs.Providers.Count == 0) 
            {
                Log.Error($"No stdev data was found for city with ID {cityId} within period {fromDate.Date} - {toDate.Date}");
                return NotFound(new ErrorHandlingModel
                {
                    Error= 404,
                    Message="Not Found",
                    Description="No data was found in given period"
                    
                });
            }
            return Ok(allStdevs);
        }
        /// <summary>
        /// Get City average
        /// </summary>
        [HttpGet("average/{CityId}")]
        public ActionResult<WeatherCityAverage> GetCityAverage(long cityId, DateTime fromDate, DateTime toDate)
        {
            if (fromDate == default)
            {
                fromDate = DateTime.Now.Date;
            }

            if (toDate == default)
            {
                toDate = DateTime.Now.Date;
            }

            if ((toDate - fromDate).TotalDays > 14 || (toDate - fromDate).TotalDays < 0)
            {
                Log.Error($"No data was provided for city with Id {cityId} as time period was too big {fromDate.Date} - {toDate.Date}");
                return BadRequest(new ErrorHandlingModel
                {
                    Error = 400,
                    Description = "Bad Request",
                    Message = "Time between days can't be negative or be higher than 14"
                });
            }

            var average =
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

            var result = new WeatherCityAverage
            {
                CityId = cityId,
                FromDate = fromDate.Date,
                ToDate = toDate.Date,
                Average = average
            };
            if (result.Average.Count == 0)
            {
                Log.Error($"No averages found for city with ID {cityId} within period {fromDate.Date} - {toDate.Date}");
                return NotFound(new ErrorHandlingModel
                {
                    Error = 404,
                    Message = "Not Found",
                    Description = "No data was found in given period"
                });
            }
            return Ok(result);
        }
        /// <summary>
        /// Gets forecast for specific city
        /// </summary>
        [HttpGet("{CityId}")]
        public ActionResult<WeatherRawForecasts> GetAllForecasts(long cityId, DateTime fromDate, DateTime toDate)
        {
            if (fromDate == default)
            {
                fromDate = DateTime.Now.Date;
            }
                
            if (toDate == default)
            {
                toDate = DateTime.Now.Date;
            }

            if ((toDate - fromDate).TotalDays > 14 || (toDate - fromDate).TotalDays < 0)
            {
                Log.Error($"No data was provided for city with Id {cityId} as time period was too big {fromDate.Date} - {toDate.Date}");
                return BadRequest(new ErrorHandlingModel
                {
                    Error = 400,
                    Message = "Bad Request",
                    Description = "Time between days can't be negative or be higher than 14"
                });
            }
            var providersWithForecasts = _context.Forecasts
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
                    .OrderBy(order=> order.Date)
                    .ToList()
                }).ToList();

            var actualTemperature = _context.ActualTemperatures
                .Where(actualTemperature => actualTemperature.CitiesId == cityId
                && actualTemperature.ForecastTime >= fromDate.Date 
                && actualTemperature.ForecastTime <= toDate.Date)
                .Select(actualTemprature=> new FactualTemperature
                {
                    Date= actualTemprature.ForecastTime,
                    Temperature= actualTemprature.Temperature
                })
                .OrderBy(temperature=> temperature.Date)
                .ToList();

            var forecasts = new WeatherRawForecasts
            {
                CityId = cityId,
                FromDate = fromDate,
                ToDate = toDate,
                FactualTemperature = actualTemperature,
                Providers = providersWithForecasts
            };
            if (forecasts.Providers.Count == 0)
            {
                Log.Error($"No raw data was found for city with ID {cityId} within period {fromDate.Date} - {toDate.Date}");
                return NotFound( new ErrorHandlingModel
                {
                    Error = 404,
                    Message = "Not Found",
                    Description = "No data was found in given period"
                });
            }
                

            return Ok(forecasts);
        }


        double? GetStdev(double? averageActualTemperature, List<Forecasts> forecasts, DateTime date, string provider)
        {

            var mean = averageActualTemperature;

            if (mean == null) return null;

            var sum = forecasts
                        .Where(i => i.ForecastTime.Date == date && i.Provider == provider)
                        .Sum(i => i.Temperature - mean) * forecasts
                        .Where(i => i.ForecastTime.Date == date && i.Provider == provider)
                        .Sum(i => i.Temperature - mean);

            var count = forecasts
                .Where(i => i.ForecastTime.Date == date && i.Provider == provider).Count();

            return Math.Sqrt((double)(sum / count));

        }
        List<StdevsProviders> GetProvidersWithStdevs(DateTime fromDate, DateTime toDate, long cityId, List<ActualTemperature> actualTemperature, List<Forecasts> forecasts, double? averageActualTemperature)
        {
            List<StdevsProviders> providers = forecasts
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
            return providers;
        }
        List<Forecasts> GetForecasts(DateTime fromDate, DateTime toDate, long cityId)
        {
            var forecasts = _context.Forecasts
                .Where(forecastInfo =>
                   forecastInfo.CitiesId == cityId
                && forecastInfo.ForecastTime.Date >= fromDate.Date
                && forecastInfo.ForecastTime.Date <= toDate.Date)
                .AsNoTracking()
                .ToList();
            return forecasts;
        }
        List<ActualTemperature> GetActualTemperatures(DateTime fromDate, DateTime toDate, long cityId)
        {
            var actualTemperature = _context.ActualTemperatures
                .Where(ActualTemperature =>
                   ActualTemperature.CitiesId == cityId
                && ActualTemperature.ForecastTime.Date >= fromDate.Date
                && ActualTemperature.ForecastTime.Date <= toDate.Date)
                .AsNoTracking()
                .ToList();
            return actualTemperature;
        }
    }
}