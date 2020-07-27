using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WeatherForecastAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
namespace WeatherForecastAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CitiesController : ControllerBase
    {
        /// <summary>
        /// Gets All cities
        /// </summary>
        /// private WeatherContext _context;
        private readonly WeatherContext _context;
        public CitiesController(WeatherContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Get list of all cities
        /// </summary>
        [HttpGet]
        public ActionResult<CitiesResponseModel> GetAllCities()
        {
            CitiesResponseModel allCities = new CitiesResponseModel
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
            return allCities;
        }
    }
}
