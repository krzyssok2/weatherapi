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
                         CityId=1,
                         CityName = "vilnius",
                         Country ="lithuania"
                     },
                     new CityInfoResponseModel
                     {
                         CityId=2,
                         CityName = "kaunas",
                         Country ="lithuania"
                     },
                     new CityInfoResponseModel
                     {
                         CityId=3,
                         CityName = "alytus",
                         Country ="lithuania"
                     },
                     new CityInfoResponseModel
                     {
                         CityId=4,
                         CityName = "palanga",
                         Country ="lithuania"
                     }
                 }
            };
            return MyResponse;
        }
        /// <summary>
        /// Edit Existing city data
        /// </summary>
        /// <param name="CityId"></param>
        /// <returns></returns>
        [HttpPost("{CityId}")]
        public ActionResult<CityInfoResponseModel> PostCity(string CityId)
        {
            CityInfoResponseModel MyResponse = new CityInfoResponseModel
            {
                CityId = 1,
                CityName = "vilnius",
                Country = "lithuania"
            };
            return Ok(MyResponse);
        }
        /// <summary>
        /// Delete City
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{CityId}")]
        public ActionResult<CityInfoResponseModel> DeleteCity()
        {
            CityInfoResponseModel MyResponse = new CityInfoResponseModel
            {
                CityId = 1,
                CityName = "vilnius",
                Country = "lithuania"
            };
            return Ok(MyResponse);
        }
    }
}
