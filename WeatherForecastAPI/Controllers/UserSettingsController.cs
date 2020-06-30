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
    public class UserSettingsController
    {
        //[HttpGet("")]
        //public ActionResult<AllUsers> GetAllUsers()
        //{
        //    AllUsers Allusers = new AllUsers
        //    {
        //        UserAccounts = new List<UserAccount>
        //        {
        //            new UserAccount
        //            {
        //                Username="A",
        //                UserId = 1,
        //            },
        //            new UserAccount
        //            {
        //                Username="B",
        //                UserId = 2,
        //            }
        //        }
        //    };
        //    return Allusers;
        //}
        //[HttpPost("")]
        //public ActionResult<UserAccount> PostNewUser()
        //{
        //    UserAccount user = new UserAccount
        //    {
        //        Username = "Who?",
        //        UserId = 12512
        //    };
        //    return user;
        //}
        /// <summary>
        /// Get prefered unit of the user
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [HttpGet("unit")]
        public ActionResult<PreferedTemperature> GettingPreferedUnit(long userid)
        {
            PreferedTemperature preferedTemperature = new PreferedTemperature
            {
                PrefferedUnit = Temperature.C
            };
            return preferedTemperature;
        }
        /// <summary>
        /// Edit prefered unit
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [HttpPut("unit")]
        public ActionResult<PreferedTemperature> PutPreferedUnit(long userid)
        {
            PreferedTemperature preferedTemperature = new PreferedTemperature
            {
                PrefferedUnit = Temperature.F
            };
            return preferedTemperature;
        }
        /// <summary>
        ///User's favorite cities 
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [HttpGet("favorite-cities")]
        public ActionResult<List<PrefferedCities>> GettingFavoriteCities(long userid)
        {
            List<PrefferedCities> prefferedCities = new List<PrefferedCities>
            {
                new PrefferedCities
                {
                    CityId=1
                },
                new PrefferedCities
                {
                    CityId=2
                }
            };
            return prefferedCities;
        }
        /// <summary>
        /// Edit user's favorite cities
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [HttpPut("favorite-cities")]
        public ActionResult<List<PrefferedCities>> PutPreferedCities(long userid)
        {
            List<PrefferedCities> prefferedCities = new List<PrefferedCities>
            {
                new PrefferedCities
                {
                    CityId=1
                },
                new PrefferedCities
                {
                    CityId=2
                }
            };
            return prefferedCities;
        }
        [HttpGet("")]
        public ActionResult<UsersSettings> GetAllSettings(long userid)
        {
            UsersSettings prefferedCities = new UsersSettings
            {
                PrefferedUnit=0,
                FavoriteCities= new List<PrefferedCities>
                {
                    new PrefferedCities
                    {
                        CityId=1
                    }
                }
            };
            return prefferedCities;
        }

    }
}
