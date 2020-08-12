using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WeatherForecastAPI.Models;
using WeatherForecastAPI.Entities;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using WeatherForecastAPI.Models.Swagger;
using WeatherForecastAPI.Services;
using System.Xml.Schema;

namespace WeatherForecastAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherContext _context;
        private readonly WeatherServices service;
        private readonly ConverterService converter;
        public WeatherController(WeatherContext context)
        {
            _context = context;
            service = new WeatherServices(_context);
            converter = new ConverterService();
        }


        /// <summary>
        /// Get stdev of the city
        /// </summary>
        [Authorize]
        [HttpGet("stdev/{cityId}")]
        [ProducesResponseType(typeof(WeatherErrorsResponse), 400)]
        [ProducesResponseType(typeof(AllStdevs), 200)]
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
                    Error = HandlingErrors.TimeSpanTooLong
                });
            }

            var userName = User.Claims.Single(a => a.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            var unit = _context.UserSettings.First(x => x.User == userName).Units;

            var forecasts = service.GetForecasts(fromDate, toDate, cityId);
            foreach(var item in forecasts)
            {
                item.Temperature = converter.ConvertetFromCelcius(item.Temperature, unit);
            };


            var actualTemperature = service.GetActualTemperatures(fromDate, toDate, cityId);
            foreach(var item in actualTemperature)
            {
                item.Temperature = converter.ConvertetFromCelcius(item.Temperature, unit);
            }

            var averageActualTemperature = actualTemperature.Average(i =>(double?) i.Temperature);
            var providers = service.GetProvidersWithStdevs(actualTemperature, forecasts, averageActualTemperature);

            var allStdevs = new AllStdevs
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
            return Ok(allStdevs);
        }
        /// <summary>
        /// Get City average
        /// </summary>
        [HttpGet("average/{cityId}")]
        [ProducesResponseType(typeof(WeatherCityAverage), 200)]
        [ProducesResponseType(typeof(WeatherErrorsResponse), 400)]
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
                    Error = HandlingErrors.TimeSpanTooLong
                });
            }

            var cityAverageByDay = service.GetCityAverageByDay(fromDate, toDate, cityId);

            var userName = User.Claims.Single(a => a.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            var unit = _context.UserSettings.First(x => x.User == userName).Units;

            foreach (var item in cityAverageByDay)
            {
                item.Average = converter.ConvertetFromCelcius(item.Average, unit);
            }

            var result = new WeatherCityAverage
            {
                CityId = cityId,
                FromDate = fromDate.Date,
                ToDate = toDate.Date,
                Average = cityAverageByDay
            };
            return Ok(result);
        }
        /// <summary>
        /// Gets forecast for specific city
        /// </summary>
        [HttpGet("{cityId}")]
        [ProducesResponseType(typeof(WeatherRawForecasts), 200)]
        [ProducesResponseType(typeof(WeatherErrorsResponse), 400)]
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
                    Error = HandlingErrors.TimeSpanTooLong
                });
            }

            var providersWithForecasts = service.GetProvidersWithForecasts(fromDate, toDate, cityId);


            var userName = User.Claims.Single(a => a.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            var unit = _context.UserSettings.First(x => x.User == userName).Units;

            var actualTemperature = service.GetActualTemperaturesTransformed(fromDate, toDate, cityId);
            foreach(var actual in actualTemperature)
            {
                actual.Temperature = converter.ConvertetFromCelcius(actual.Temperature, unit);
            }
            foreach(var item in providersWithForecasts)
            {
                foreach(var forecast in item.Forcasts)
                {
                    forecast.Temperature = converter.ConvertetFromCelcius(forecast.Temperature, unit);
                }
            }
            var forecasts = new WeatherRawForecasts
            {
                CityId = cityId,
                FromDate = fromDate,
                ToDate = toDate,
                FactualTemperature = actualTemperature,
                Providers = providersWithForecasts
            };        
            return Ok(forecasts);
        }
    }
}