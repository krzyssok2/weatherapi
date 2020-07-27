using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Internal.Account.Manage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherForecastAPI.Entities;
using WeatherForecastAPI.Migrations;
using WeatherForecastAPI.Models;

namespace WeatherForecastAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SettingsController: ControllerBase
    {
        private readonly WeatherContext _context;
        public SettingsController(WeatherContext context)
        {
            _context = context;
        }

        /// <summary>
        ///User's favorite cities 
        /// </summary>
        /// <returns></returns>
        [HttpGet("favorite-cities")]
        public ActionResult<AllPreferedCities> GettingFavoriteCities()
        {
            AllPreferedCities prefferedCities = new AllPreferedCities();

            var userName = User.Claims.Where(a => a.Type == System.Security.Claims.ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            var cities = _context.FavoriteCities
                .Where(i => i.User.User == userName)
                .Select(i => new PreferedCities
                {
                    CityId=i.City.Id,
                    CityName=i.City.Name,
                    Country=i.City.Country
                }).ToList();
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
        public ActionResult<AllPreferedCities> PutPreferedCities([FromBody] InsertCities insertCities)
        {
            var userName = User.Claims.Where(a => a.Type == System.Security.Claims.ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            var settings = _context.UserSettings.Include(x => x.FavoriteCities).FirstOrDefault(x => x.User == userName);

            settings.FavoriteCities.Clear();

            var cities = _context.Cities.ToList();
            foreach (var city in insertCities.CitiesId)
            {
                settings.FavoriteCities.Add(new FavoriteCities
                {
                    UserId = settings.Id,
                    City = cities.FirstOrDefault(i => i.Id == city.CityId)
                });
            }

            _context.SaveChanges();
            AllPreferedCities preferedCities = new AllPreferedCities();
            return Ok();
        }
        /// <summary>
        /// Delete city from your favorite list
        /// </summary>
        [HttpDelete("favorite-cities/{CityId}")]
        public ActionResult<AllPreferedCities> DeletePreferedCity(long CityId)
        {
            var userId = User.Claims.Where(a => a.Type == System.Security.Claims.ClaimTypes.NameIdentifier).FirstOrDefault().Value;

            var city = _context.Cities.Where(c => c.Id == CityId).FirstOrDefault();
            if (city == null) return BadRequest("Such City Doesnt exist");

            var user = _context.UserSettings.First(u => u.User == userId);

            var deletethis = _context.FavoriteCities.Where(fc => fc.City == city && fc.User == user).FirstOrDefault();

            var delete = _context.FavoriteCities.Remove(deletethis);

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
            var userName = User.Claims.Where(a => a.Type == System.Security.Claims.ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            Settings settings = _context.UserSettings
                .Where(i => i.User == userName)
                .Select(i => new Settings
                {
                    PreferedUnit=i.Units,
                    FavoriteCities = i.FavoriteCities.Select(j=> new PreferedCities
                    {
                        CityId=j.City.Id,
                        CityName=j.City.Name,
                        Country=j.City.Country
                    }).ToList()
                }).First();
            return Ok(settings);
        }
        /// <summary>
        /// Update all settings
        /// </summary>
        /// <returns></returns>
        [HttpPut("")]
        public ActionResult UpdateAllSettings([FromBody]InsertSettings insertSettings)
        {
            var userName = User.Claims.Where(a => a.Type == System.Security.Claims.ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            var settings = _context.UserSettings.Include(x => x.FavoriteCities).FirstOrDefault(x => x.User == userName);
            settings.FavoriteCities.Clear();

            var cities = _context.Cities.ToList();
            foreach (var city in insertSettings.CitiesId)
            {
                settings.FavoriteCities.Add(new FavoriteCities
                {
                    UserId = settings.Id,
                    City = cities.FirstOrDefault(i => i.Id == city.CityId)
                });
            }
            settings.Units = insertSettings.Units;

            _context.SaveChanges();
            AllPreferedCities preferedCities = new AllPreferedCities();
            return Ok();
        }

    }
}
