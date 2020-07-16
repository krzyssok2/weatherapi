﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WeatherForecastAPI.Models;
using WeatherForecastAPI.Entities;
using Newtonsoft.Json;
using System.Xml;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using WeatherForecastAPI.Worker;

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
                return BadRequest(new ErrorHandlingModel
                {
                    Error = 400,
                    Message = "Bad Request",
                    Description = "Time between days can't be negative or be higher than 14"
                });
            }
            var forecasts = GetForecasts(fromDate, toDate, cityId);
            var actualTemperature = GetActualTemperatures(fromDate, toDate, cityId);
            var averageActualTemperature = actualTemperature.Average(i => i.Temperature);
            var providers = GetProvidersWithStdevs(fromDate, toDate, cityId, actualTemperature, forecasts, averageActualTemperature);

            AllStdevs AllStdevs = new AllStdevs
            {
                CityId = cityId,
                FromDate = fromDate,
                ToDate = toDate,
                Providers= providers
            };
            foreach(var provider in AllStdevs.Providers.ToList())
            {
                foreach (var stdev in provider.Stdevs.ToList())
                {
                    if (stdev.Factual == null)
                    {
                        provider.Stdevs.Remove(stdev);
                    }
                }
            }
            if (AllStdevs.Providers.Count == 0) 
            {
                return NotFound(new ErrorHandlingModel
                {
                    Error= 404,
                    Message="Not Found",
                    Description="No data was found in given period"
                    
                });
            }
            return Ok(AllStdevs);
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
                return NotFound( new ErrorHandlingModel
                {
                    Error = 404,
                    Message = "Not Found",
                    Description = "No data was found in given period"
                });
            }
                

            return Ok(forecasts);
        }






        double? GetStdev(double? AverageActualTemperature, List<Forecasts> forecasts, DateTime date, string provider)
        {

            var Mean = AverageActualTemperature;

            if (Mean == null) return null;

            var Sum = forecasts
                        .Where(i => i.ForecastTime.Date == date && i.Provider == provider)
                        .Sum(i => i.Temperature - Mean) * forecasts
                        .Where(i => i.ForecastTime.Date == date && i.Provider == provider)
                        .Sum(i => i.Temperature - Mean);

            var Count = forecasts
                .Where(i => i.ForecastTime.Date == date && i.Provider == provider).Count();

            return Math.Sqrt((double)(Sum / Count));

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