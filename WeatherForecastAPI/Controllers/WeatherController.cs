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
