using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherForecastAPI.Models;

namespace WeatherForecastAPI.Controllers
{
    [Route("api/[controller]")]
    public class CitiesController : ControllerBase
    {
        /// <summary>
        /// Gets All cities
        /// </summary>
        [HttpGet("")]
        public ActionResult<CitiesResponseModel> GetAllCities()
        {
            CitiesResponseModel MyResponse = new CitiesResponseModel
            {
                Cities = new List<CityInfoResponseModel>
                 {
                     new CityInfoResponseModel
                     {
                         CityName = "vilnius",
                         Country ="lithuania"
                     },
                     new CityInfoResponseModel
                     {
                         CityName = "kaunas",
                         Country ="lithuania"
                     },
                     new CityInfoResponseModel
                     {
                         CityName = "alytus",
                         Country ="lithuania"
                     },
                     new CityInfoResponseModel
                     {
                         CityName = "palanga",
                         Country ="lithuania"
                     }
                 }
            };
            return MyResponse;
        }
        [HttpGet("{CityName}")]
        public ActionResult<CityInfoResponseModel> GetSpecificCity(string cityName)
        {
            CityInfoResponseModel MyCity = new CityInfoResponseModel
            {
                CityName = cityName,
                Country="lithuania"
            };
            return MyCity;
        }
    }
}
