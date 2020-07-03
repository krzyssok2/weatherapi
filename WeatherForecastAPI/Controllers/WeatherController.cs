﻿using Microsoft.AspNetCore.Mvc;
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
using System.Net.Http;
using System.Diagnostics;

namespace WeatherForecastAPI.Controllers
{
    [Route("api/[controller]")]
    public class WeatherController: ControllerBase
    {
        /// <summary>
        /// Get City average
        /// </summary>
        /// <param name="CityId"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns></returns>
        [HttpGet("average/{CityId}")]
        public ActionResult<WeatherCityAverage> GetCityAverage(long CityId, DateTime FromDate, DateTime ToDate)
        {
            Random random = new Random();
            WeatherCityAverage CityAverage = new WeatherCityAverage
            {
                CityId=1,
                Average= new List<CityAverageByDay>
                {
                    new CityAverageByDay
                    {
                        Date="",
                        Average=5
                    },
                    new CityAverageByDay
                    {
                        Date="",
                        Average=12
                    }
                }
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
        /// <summary>
        /// Get stdev of the city
        /// </summary>
        /// <param name="CityId"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns></returns>
        [HttpGet("stdev/{CityId}")]
        public ActionResult<AllStdevs> GetAllStdevsFrom(long CityId, DateTime FromDate, DateTime ToDate)
        {
            AllStdevs AllStdevs = new AllStdevs
            {
                
            };
            return AllStdevs;
        }/// <summary>
        /// Gets forecast for specific city
        /// </summary>
        /// <param name="CityId"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns></returns>
        [HttpGet("{CityId}")]
        public ActionResult<WeatherRawForecasts> GetAllForecasts(long CityId, DateTime FromDate, DateTime ToDate)
        {
            WeatherRawForecasts forecasts = new WeatherRawForecasts
            {

            };
            return forecasts;
        }




        private IHttpClientFactory _httpClientFactory;
        public WeatherController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        [HttpGet("test/OWM/{CityId}")]
        public async Task<ActionResult> GetTestOWM(string CityId)
        {
            OWMForecastRootObject weatherinfo = await TestAsync<OWMForecastRootObject>("OWM", string.Format("forecast?q={0}&units=metric&appid=4bd458b0d9e2bfadbed92b6b73ce4274", CityId), false);
            ForecastGeneralized forecastGeneralized = new ForecastGeneralized
            {
                Name = weatherinfo.city.name.ToLower(),
                CreationDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss")),
                Provider = "OWM",
                Forecasts = new List<Forecasts>()
            };
            foreach (var x in weatherinfo.list)
            {
                Forecasts item = new Forecasts
                {
                    temperature = x.main.temp,
                    ForecastTime = x.dt_txt
                };
                forecastGeneralized.Forecasts.Add(item);
            }
            return Ok(forecastGeneralized);
        }
        [HttpGet("test/METEO/{CityId}")]
        public async Task<ActionResult> GetTestMETEO(string CityId)
        {
            MeteoRootObject weatherinfo = await TestAsync<MeteoRootObject>("METEO", string.Format("places/{0}/forecasts/long-term", CityId), false);

            ForecastGeneralized forecastGeneralized = new ForecastGeneralized
            {
                Name = weatherinfo.place.name.ToLower(),
                Provider="Meteo",
                CreationDate = weatherinfo.forecastCreationTimeUtc,
                Forecasts= new List<Forecasts>()
            };
            foreach (var x in weatherinfo.forecastTimestamps)
            {
                Forecasts item = new Forecasts
                {
                    ForecastTime = x.forecastTimeUtc,
                    temperature = x.airTemperature
                };
                forecastGeneralized.Forecasts.Add(item);
            }

            return Ok(forecastGeneralized);
        }
        [HttpGet("test/BBC/{CityId}")]
        public ActionResult GetTestBBC(string CityId)
        {
            using WebClient client = new WebClient();
            string doc = client.DownloadString(Uri.EscapeUriString("https://www.bbc.com/weather/593116?day=1/application/json"));
            doc = doc.Substring(doc.IndexOf("application/json"));
            doc = doc.Substring(doc.IndexOf("{"));
            doc = doc.Substring(0, doc.IndexOf("<"));
            BBCScrapRootObject weatherinfo = JsonConvert.DeserializeObject<BBCScrapRootObject>(doc);
            ForecastGeneralized forecastGeneralized = new ForecastGeneralized
            {
                Name = weatherinfo.data.location.name,
                Provider = "BBC",
                CreationDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss")),
                Forecasts = new List<Forecasts>()
            };
            foreach (var x in weatherinfo.data.forecasts)
            {
                foreach (var y in x.detailed.reports)
                {
                    Forecasts item = new Forecasts
                    {
                        ForecastTime = Convert.ToDateTime(Convert.ToString(y.localDate + "T" + y.timeslot)),
                        temperature = y.temperatureC
                    };
                    forecastGeneralized.Forecasts.Add(item);
                }
            }
            return Ok(forecastGeneralized);


        }
        public async Task<T> TestAsync<T>(string provider, string path, bool IsXML)
        {
            var client = _httpClientFactory.CreateClient(provider);
            var result = await client.GetStringAsync(path);
            if(IsXML==true)
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(result);
                result = JsonConvert.SerializeXmlNode(doc);
            }
            T MyClass = JsonConvert.DeserializeObject<T>(result);
            return MyClass;
        }
        #region Providers
        //[HttpGet("city/{CityName}/provider/OWM")]
        //public ActionResult<OWMRootObject> FetchOWMCurrentData(string CityName)
        //{
        //    string appId = "4bd458b0d9e2bfadbed92b6b73ce4274";
        //    string url = string.Format("http://api.openweathermap.org/data/2.5/weather?q={0}&units=metric&cnt=1&APPID={1}", CityName, appId);
        //    using (WebClient client = new WebClient())
        //    {
        //        string json = client.DownloadString(url);
        //        OWMRootObject weatherinfo = JsonConvert.DeserializeObject<OWMRootObject>(json);
        //        return weatherinfo;
        //    }
        //}
        //[HttpGet("city/{CityName}/provider/METEO")]
        //public ActionResult<MeteoRootObject> FetchMETEOCurrentData(string CityName)
        //{
        //    string url = string.Format("https://api.meteo.lt/v1/places/{0}/forecasts/long-term", CityName);
        //    using (WebClient client = new WebClient())
        //    {
        //        string json = client.DownloadString(url);
        //        MeteoRootObject weatherinfo = JsonConvert.DeserializeObject<MeteoRootObject>(json);
        //        return weatherinfo;
        //    }
        //}
        //[HttpGet("city/{CityName}/provider/BBC")]
        //public ActionResult<BBCRootObject> FetchBBCCurrentData(string CityName)
        //{
        //    string url = string.Format("https://weather-broker-cdn.api.bbci.co.uk/en/forecast/rss/3day/593116");
        //    using (WebClient client = new WebClient())
        //    {
        //        string xml = client.DownloadString(url);
        //        XmlDocument doc = new XmlDocument();
        //        doc.LoadXml(xml);
        //        string json = JsonConvert.SerializeXmlNode(doc);

        //        BBCRootObject weatherinfo = JsonConvert.DeserializeObject<BBCRootObject>(json);
        //        return weatherinfo;
        //    }
        //}
        #endregion
    }
}
