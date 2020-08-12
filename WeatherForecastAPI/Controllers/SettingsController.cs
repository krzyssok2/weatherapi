using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WeatherForecastAPI.Entities;
using WeatherForecastAPI.Models;
using WeatherForecastAPI.Models.Swagger;
using WeatherForecastAPI.Services;

namespace WeatherForecastAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SettingsController : ControllerBase
    {
        private readonly WeatherContext _context;
        private readonly SettingsServices service;
        public SettingsController(WeatherContext context)
        {
            _context = context;
            service = new SettingsServices();
        }

        /// <summary>
        ///User's favorite cities
        /// </summary>
        /// <returns></returns>
        [HttpGet("favorite-cities")]
        public ActionResult<AllPreferedCities> GetFavoriteCities()
        {
            var prefferedCities = new AllPreferedCities();

            var userName = User.Claims.Single(a => a.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;

            var cities = service.GetPreferedCities(_context.FavoriteCities.Include(i=>i.City).Include(i=>i.User).ToList(), userName);

            var preferedCities = new AllPreferedCities
            {
                PreferedCities = cities
            };
            return Ok(preferedCities);
        }
        /// <summary>
        /// Edit user's favorite cities
        /// </summary>
        /// <returns></returns>
        [HttpPut("favorite-cities")]
        public ActionResult PutPreferedCities( InsertCities insertCities)
        {
            var userName = User.Claims.Single(a => a.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            var settings = _context.UserSettings.Include(x => x.FavoriteCities).FirstOrDefault(x => x.User == userName);

            settings.FavoriteCities.Clear();

            var cities = _context.Cities.ToList();
            foreach (var city in insertCities.CitiesId)
            {
                if (cities.FirstOrDefault(i => i.Id == city.CityId) == null) continue;
                settings.FavoriteCities.Add(new FavoriteCities
                {
                    UserId = settings.Id,
                    City = cities.FirstOrDefault(i => i.Id == city.CityId)
                });
            }

            _context.SaveChanges();
            return Ok();
        }
        /// <summary>
        /// Delete city from your favorite list
        /// </summary>
        /// <response code="200">Success</response>   
        [HttpDelete("favorite-cities/{CityId}")]
        [ProducesResponseType(typeof(CityErrorsResponse), 400)]
        public ActionResult DeletePreferedCity(long CityId)
        {
            var userId = User.Claims.Single(a => a.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;

            var city = _context.Cities.Where(c => c.Id == CityId).FirstOrDefault();
            if (city == null)
            {
                return BadRequest(new CityErrorsResponse
                {
                    Error = CityErrors.NoDataFound
                });
            }

            var user = _context.UserSettings.First(u => u.User == userId);
            var deletecity = _context.FavoriteCities.Where(fc => fc.City == city && fc.User == user).FirstOrDefault();
            var delete = _context.FavoriteCities.Remove(deletecity);

            _context.SaveChanges();

            return Ok();
        }
        /// <summary>
        /// Get all settings
        /// </summary>
        /// <returns></returns>
        [HttpGet("")]
        public ActionResult<Settings> GetAllSettings()
        {
            var userName = User.Claims.Single(a => a.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;

            Settings settings = _context.UserSettings
                .Where(i => i.User == userName)
                .Select(i => new Settings
                {
                    PreferedUnit = i.Units,
                    FavoriteCities = i.FavoriteCities.Select(j => new PreferedCities
                    {
                        CityId = j.City.Id,
                        CityName = j.City.Name,
                        Country = j.City.Country
                    }).ToList()
                }).First();
            return Ok(settings);

        }
        /// <summary>
        /// Update all settings
        /// </summary>
        /// <returns></returns>
        [HttpPut("")]
        public ActionResult UpdateAllSettings( InsertSettings insertSettings)
        {
            var userName = User.Claims.Single(a => a.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            var settings = _context.UserSettings
                .Include(x => x.FavoriteCities)
                .FirstOrDefault(x => x.User == userName);
            settings.FavoriteCities.Clear();

            var cities = _context.Cities.ToList();

            foreach (var city in insertSettings.CitiesId)
            {
                if (cities.FirstOrDefault(i => i.Id == city.CityId) == null) continue;
                settings.FavoriteCities.Add(new FavoriteCities
                {
                    UserId = settings.Id,
                    City = cities.FirstOrDefault(i => i.Id == city.CityId)
                });
            }
            settings.Units = insertSettings.Units;

            _context.SaveChanges();
            return Ok();
        }
    }
}