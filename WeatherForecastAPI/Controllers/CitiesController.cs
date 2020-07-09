using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherForecastAPI.Entities;
using WeatherForecastAPI.Models;

namespace WeatherForecastAPI.Controllers
{
    [Route("api/[controller]")]
    public class CitiesController : ControllerBase
    {
        /// <summary>
        /// Gets All cities
        /// </summary>
        /// private WeatherContext _context;
        private WeatherContext _context;
        public CitiesController(WeatherContext context)
        {
            _context = context;
        }
        [HttpGet("")]
        public ActionResult<CitiesResponseModel> GetAllCities()
        {
            CitiesResponseModel MyResponse = new CitiesResponseModel
            {
                Cities = new List<CityInfoResponseModel>()
            };
            foreach (var obj in _context.Cities.Include(i=>i.Forecasts))
            {
                var a = new CityInfoResponseModel
                {
                    CityId = obj.Id,
                    CityName = obj.Name,
                    Country = obj.Country,
                    FromDate = obj.Forecasts.Min(i => i.ForecastTime),
                    ToDate = obj.Forecasts.Max(i => i.ForecastTime)
                };
                MyResponse.Cities.Add(a);
            }
            
            return MyResponse;
        }
    }
}
