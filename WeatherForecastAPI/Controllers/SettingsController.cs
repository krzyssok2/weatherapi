using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WeatherForecastAPI.Models;

namespace WeatherForecastAPI.Controllers
{
    [Route("api/[controller]")]
    public class SettingsController: ControllerBase
    {
        /// <summary>
        ///User's favorite cities 
        /// </summary>
        /// <returns></returns>
        [HttpGet("favorite-cities")]
        public ActionResult<AllPreferedCities> GettingFavoriteCities()
        {
            AllPreferedCities prefferedCities = new AllPreferedCities();
            return prefferedCities;
        }
        /// <summary>
        /// Edit user's favorite cities
        /// </summary>
        /// <returns></returns>
        [HttpPut("favorite-cities")]
        public ActionResult<AllPreferedCities> PutPreferedCities()
        {
            AllPreferedCities preferedCities = new AllPreferedCities
            {

            }
            ;
            return Ok();
        }
        [HttpDelete("favorite-cities/{CityId}")]
        public ActionResult<AllPreferedCities> DeletePreferedCity()
        {
            AllPreferedCities prefferedCities = new AllPreferedCities();
            return Ok();
        }
        /// <summary>
        /// Get all settings
        /// </summary>
        /// <returns></returns>
        [HttpGet("")]
        public ActionResult<Settings> GetAllSettings()
        {
            Settings preferedCities = new Settings
            {
                PreferedUnit=0,
                FavoriteCities= new List<PreferedCities>
                {
                    new PreferedCities
                    {
                        CityId=1
                    }
                }
            };
            return preferedCities;
        }
        /// <summary>
        /// Update all settings
        /// </summary>
        /// <returns></returns>
        [HttpPut("")]
        public ActionResult<Settings> UpdateAllSettings()
        {
            Settings preferedCities = new Settings
            {
                PreferedUnit = 0,
                FavoriteCities = new List<PreferedCities>
                {
                    new PreferedCities
                    {
                        CityId=1
                    }
                }
            };
            return preferedCities;
        }

    }
}
