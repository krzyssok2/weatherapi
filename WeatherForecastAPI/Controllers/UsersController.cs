using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherForecastAPI.Models;

namespace WeatherForecastAPI.Controllers
{
    [Route("api/[controller]")]
    public class UsersController
    {
        [HttpGet("")]
        public ActionResult<AllUsers> GetAllUsers()
        {
            AllUsers Allusers = new AllUsers
            {
                UserAccounts = new List<UserAccount>
                {
                    new UserAccount
                    {
                        UserId = 1,
                        UserSettings = new UsersSettings
                        {
                            PreferanceC = true,
                            PrefferedCities = new List<PrefferedCities>
                            {
                                new PrefferedCities
                                {
                                    CityName="vilnius",
                                    Country="lithuania"
                                },
                                new PrefferedCities
                                {
                                    CityName="kaunas",
                                    Country="lithuania"
                                },
                                new PrefferedCities
                                {
                                    CityName="alytus",
                                    Country="lithuania"
                                }
                            }
                        }
                    },
                    new UserAccount
                    {
                        UserId = 2,
                        UserSettings = new UsersSettings
                        {
                            PreferanceC = true,
                            PrefferedCities = new List<PrefferedCities>
                            {
                                new PrefferedCities
                                {
                                    CityName="vilnius",
                                    Country="lithuania"
                                },
                                new PrefferedCities
                                {
                                    CityName="alytus",
                                    Country="lithuania"
                                }
                            }
                        }
                    }
                }
            };
            return Allusers;
        }
        [HttpGet("{UserId}")]
        public ActionResult<UserAccount> GetSettingsByUser(long userid)
        {
            UserAccount User = new UserAccount
            {
                UserId = userid,
                UserSettings = new UsersSettings
                {
                    PreferanceC = true,
                    PrefferedCities = new List<PrefferedCities>
                    {
                        new PrefferedCities
                        {
                            CityName="vilnius",
                            Country="lithuania"
                        },
                        new PrefferedCities
                        {
                            CityName="kaunas",
                            Country="lithuania"
                        },
                        new PrefferedCities
                        {
                            CityName="alytus",
                            Country="lithuania"
                        }
                    }
                }
            };
            return User;
        }
        [HttpGet("{UserId}/usersettings")]
        public ActionResult<UsersSettings> GettingUserSettings(long userid)
        {
            UsersSettings Usersetting = new UsersSettings
            {
                PreferanceC = true,
                PrefferedCities = new List<PrefferedCities>
                    {
                        new PrefferedCities
                        {
                            CityName="vilnius",
                            Country="lithuania"
                        },
                        new PrefferedCities
                        {
                            CityName="kaunas",
                            Country="lithuania"
                        },
                        new PrefferedCities
                        {
                            CityName="alytus",
                            Country="lithuania"
                        }
                    }
            };
            return Usersetting;
        }

    }
}
