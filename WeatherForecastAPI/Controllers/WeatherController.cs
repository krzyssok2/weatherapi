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
    public class WeatherController: ControllerBase
    {
        [HttpGet("city/{CityName}/average/from/{FromDate}/to/{ToDate}")]
        public ActionResult<WeatherCityAverage> GetCityAverage(string cityname,string fromdate,string todate)
        {
            Random random = new Random();
            WeatherCityAverage CityAverage = new WeatherCityAverage
            {
                CityName = cityname,
                Country = "lithuania",
                FromDate = fromdate,
                ToDate= todate,
                Average = random.NextDouble() * 36
            };
            return CityAverage;
        }
        #region OldCodeAllAveragesOfAllCities
        //[HttpGet("averages/from/{FromDate}/to/{ToDate}")]
        //public ActionResult<WeatherAllAverages> GetAllCityAverages(string FromDate, string ToDate)
        //{
        //    WeatherAllAverages AllAverages = new WeatherAllAverages
        //    {
        //        AllCitiesAverage = new List<WeatherCityAverage>
        //        {
        //            new WeatherCityAverage
        //            {
        //                CityName="vilnius",
        //                Country="lithuania",
        //                FromDate=FromDate,
        //                ToDate=ToDate,
        //                Average=36
        //            },
        //            new WeatherCityAverage
        //            {
        //                CityName="kaunas",
        //                Country="lithuania",
        //                FromDate=FromDate,
        //                ToDate=ToDate,
        //                Average=36
        //            },
        //            new WeatherCityAverage
        //            {
        //                CityName="alytus",
        //                Country="lithuania",
        //                FromDate=FromDate,
        //                ToDate=ToDate,
        //                Average=36
        //            },
        //            new WeatherCityAverage
        //            {
        //                CityName="klaipeda",
        //                Country="lithuania",
        //                FromDate=FromDate,
        //                ToDate=ToDate,
        //                Average=36
        //            },
        //        }
        //    };
        //    return AllAverages;
        //}
        #endregion
        [HttpGet("stdev/city/{CityName}/from/{FromDate}/to/{ToDate}")]
        public ActionResult<AllStdevs> GetAllStdevsFrom(string CityName,string FromDate, string ToDate)
        {
            AllStdevs AllStdevs = new AllStdevs
            {
                allstevs = new List<Stdevs>
                {
                    new Stdevs
                    {
                        City=CityName,
                        Country="lituania",
                        Provider="Meteo",
                        DateFrom=FromDate,
                        DateTo=ToDate,
                        StdevsDataByDay= new List<StdevsFactualAndAverage>
                        {
                            new StdevsFactualAndAverage
                            {
                                Date="2020-05-09",
                                FactualTemperature=28,
                                StdevTemperature=18
                            },
                            new StdevsFactualAndAverage
                            {
                                Date="2020-05-10",
                                FactualTemperature=32,
                                StdevTemperature=26
                            }
                        }
                    },
                    new Stdevs
                    {
                        City=CityName,
                        Country="lituania",
                        Provider="BBC",
                        DateFrom=FromDate,
                        DateTo=ToDate,
                        StdevsDataByDay= new List<StdevsFactualAndAverage>
                        {
                            new StdevsFactualAndAverage
                            {
                                Date="2020-05-10",
                                FactualTemperature=28,
                                StdevTemperature=18
                            },
                            new StdevsFactualAndAverage
                            {
                                Date="2020-05-15",
                                FactualTemperature=32,
                                StdevTemperature=26
                            }
                        }
                    }
                }
            };
            return AllStdevs;
        }
    }
}
