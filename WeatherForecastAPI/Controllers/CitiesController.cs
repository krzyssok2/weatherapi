using Microsoft.AspNetCore.Mvc;
using System.Linq;
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
        private readonly WeatherContext _context;
        public CitiesController(WeatherContext context)
        {
            _context = context;
        }
        [HttpGet]
        public ActionResult<CitiesResponseModel> GetAllCities()
        {
            CitiesResponseModel MyResponse = new CitiesResponseModel
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
            return MyResponse;
        }
    }
}
