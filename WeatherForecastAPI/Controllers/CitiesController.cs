using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WeatherForecastAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using WeatherForecastAPI.Models.Swagger;

namespace WeatherForecastAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CitiesController : ControllerBase
    {
        /// <summary>
        /// Gets All cities
        /// </summary>
        private readonly WeatherContext _context;
        public CitiesController(WeatherContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Get list of all cities
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(CitiesResponseModel), 200)]
        public ActionResult<CitiesResponseModel> GetAllCities()
        {
            var allCities = new CitiesResponseModel
            {
                Cities = _context.Cities.Select(cityInfo => new CityInfoResponseModel
                {
                    CityId = cityInfo.Id,
                    CityName = cityInfo.Name,
                    Country = cityInfo.Country,
                    FromDate = cityInfo.Forecasts.Min(i => i.ForecastTime),
                    ToDate = cityInfo.Forecasts.Max(i => i.ForecastTime)
                }).ToList()
            };
            return Ok(allCities);
        }
    }
}
