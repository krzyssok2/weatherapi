using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WeatherForecastAPI.Models;
using Newtonsoft;
using Newtonsoft.Json;
using System.Xml;

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

        [HttpGet("city/{CityName}/provider/OWM")]
        public ActionResult<OWMRootObject> FetchOWMCurrentData(string CityName)
        {
            string appId = "4bd458b0d9e2bfadbed92b6b73ce4274";
            string url = string.Format("http://api.openweathermap.org/data/2.5/weather?q={0}&units=metric&cnt=1&APPID={1}", CityName, appId);
            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(url);
                OWMRootObject weatherinfo = JsonConvert.DeserializeObject<OWMRootObject>(json);
                return weatherinfo;
            }
        }
        [HttpGet("city/{CityName}/provider/METEO")]
        public ActionResult<MeteoRootObject> FetchMETEOCurrentData(string CityName)
        {
            string url = string.Format("https://api.meteo.lt/v1/places/{0}/forecasts/long-term", CityName);
            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(url);
                MeteoRootObject weatherinfo = JsonConvert.DeserializeObject<MeteoRootObject>(json);
                return weatherinfo;
            }
        }
        [HttpGet("city/{CityName}/provider/BBC")]
        public string FetchBBCCurrentData(string CityName)
        {
            string url = string.Format("https://weather-broker-cdn.api.bbci.co.uk/en/forecast/rss/3day/593116");
            using (WebClient client = new WebClient())
            {
                string xml = client.DownloadString(url);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                string json = JsonConvert.SerializeXmlNode(doc);
                MeteoRootObject weatherinfo = JsonConvert.DeserializeObject<MeteoRootObject>(json);
                return json;
            }
        }
    }
}
