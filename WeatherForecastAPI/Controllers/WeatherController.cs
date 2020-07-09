using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WeatherForecastAPI.Models;
using WeatherForecastAPI.Entities;
using Newtonsoft;
using Newtonsoft.Json;
using System.Xml;
using System.Net.Http;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
namespace WeatherForecastAPI.Controllers
{
    [Route("api/[controller]")]
    public class WeatherController: ControllerBase
    {
        private WeatherContext _context;
        private IHttpClientFactory _httpClientFactory;
        public WeatherController(WeatherContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }
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
            DateTime a= new DateTime();
            if ((FromDate == a && ToDate != a) || (FromDate != a && ToDate == a)) return BadRequest();
            if (FromDate == a) FromDate = DateTime.Now.Date;
            if (ToDate == a) ToDate = FromDate.AddDays(1);
            else
            {
                TimeSpan time = new TimeSpan(23, 59, 59);
                ToDate = ToDate + time;
            }
            int HowManyDays = Convert.ToInt32(Math.Abs((FromDate - ToDate).TotalDays));
            WeatherCityAverage Result = new WeatherCityAverage
            {
                CityId = CityId,
                FromDate = Convert.ToDateTime(FromDate.ToString("yyyy'-'MM'-'dd")),
                ToDate = Convert.ToDateTime(ToDate.ToString("yyyy'-'MM'-'dd")),
                Average = new List<CityAverageByDay>()
            };
            for (int i = 0; i < HowManyDays; i++)
            {
                var obj = _context.Forecasts
                    .Where(a => a.ForecastTime >= FromDate.AddDays(i) && a.ForecastTime <= FromDate.AddDays(i + 1));
                if (obj.Any())
                {
                    Result.Average.Add(new CityAverageByDay
                    {
                        Average = _context.Forecasts
                        .Where(i => i.CitiesId == CityId)
                        .Where(a => a.ForecastTime >= FromDate.AddDays(i) && a.ForecastTime <= FromDate.AddDays(i + 1))
                        .Average(i => i.Temperature),
                        Date = Convert.ToDateTime(FromDate.AddDays(i).ToString("yyyy'-'MM'-'dd"))
                    });
                }
                else
                {
                    Result.Average.Add(new CityAverageByDay
                    {
                        Average=double.NaN,
                        Date = Convert.ToDateTime(FromDate.AddDays(i).ToString("yyyy'-'MM'-'dd"))
                    });
                }
            }
            return Ok(Result);
        }
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
            TimeSpan time = new TimeSpan(23, 59, 59);
            ToDate = ToDate + time;
            int HowManyDays = Convert.ToInt32(Math.Abs((FromDate - ToDate).TotalDays));
            AllStdevs AllStdevs = new AllStdevs
            {
                CityId=CityId,
                FromDate=FromDate,
                ToDate=ToDate,
                Providers= new List<StdevsProviders>()
            };
            foreach(var obj in _context.Providers)
            {
                AllStdevs.Providers.Add(new StdevsProviders
                {
                    ProviderName=obj.ProviderName,
                    Stdevs=new List<StdevsFactualAndAverage>()
                });
            }
            double StDev = double.NaN;
            foreach(var obj in AllStdevs.Providers)
            {
                for(int i=0;i<HowManyDays;i++)
                {
                    if (_context.Forecasts
                        .Where(i => i.CitiesId == CityId&& i.Provider==obj.ProviderName)
                        .Where(a => a.ForecastTime >= FromDate.AddDays(i) && a.ForecastTime <= FromDate.AddDays(i + 1)).Any())
                    {
                        double Mean =
                            (_context.Forecasts
                            .Where(i => i.CitiesId == CityId && i.Provider == obj.ProviderName)
                            .Where(a => a.ForecastTime >= FromDate.AddDays(i) && a.ForecastTime <= FromDate.AddDays(i + 1))
                            .Average(i => i.Temperature));
                         
                        double Sum =
                            _context.Forecasts
                            .Where(i => i.CitiesId == CityId && i.Provider == obj.ProviderName)
                            .Where(a => a.ForecastTime >= FromDate.AddDays(i) && a.ForecastTime <= FromDate.AddDays(i + 1))
                            .Sum(i => (i.Temperature - Mean) * (i.Temperature - Mean));
                        double div =
                            _context.Forecasts
                            .Where(i => i.CitiesId == CityId && i.Provider == obj.ProviderName)
                            .Where(a => a.ForecastTime >= FromDate.AddDays(i) && a.ForecastTime <= FromDate.AddDays(i + 1))
                            .Count();

                        StDev = Math.Sqrt(Sum / div);
                    }
                        
                    obj.Stdevs.Add(new StdevsFactualAndAverage
                    {
                        Date=FromDate.AddDays(i),
                        Factual=double.NaN,
                        Stdev=StDev
                    });
                    StDev = double.NaN;
                }
            }
            return Ok(AllStdevs);
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
            WeatherRawForecasts forecasts = new WeatherRawForecasts();
            return forecasts;
        }


        [HttpGet("test/BBC/{CityId}")]
        public ActionResult GetTestBBC(string CityId)
        {
            var UniqueProviderID = _context.CityProviderID
                .Where(cp => cp.City.Name == CityId && cp.Provider.ProviderName == "BBC")
                .Select(i => new { i.UniqueCityID})
                .ToList();
            if (UniqueProviderID.FirstOrDefault() == null) return NotFound("Not found");

            using WebClient client = new WebClient();
            string doc = client.DownloadString(Uri.EscapeUriString("https://www.bbc.com/weather/593116?day=1/application/json"));
            doc = doc.Substring(doc.IndexOf("application/json"));
            doc = doc.Substring(doc.IndexOf("{"));
            doc = doc.Substring(0, doc.IndexOf("<"));
            BBCScrapRootObject weatherinfo = JsonConvert.DeserializeObject<BBCScrapRootObject>(doc);
            ForecastGeneralized forecastGeneralized = new ForecastGeneralized
            {
                Name = weatherinfo.data.location.name.ToLower(),
                Provider = "BBC",
                CreationDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss")),
                Forecasts = new List<ForecastsG>()
            };
            foreach (var x in weatherinfo.data.forecasts)
            {
                foreach (var y in x.detailed.reports)
                {
                    ForecastsG item = new ForecastsG
                    {
                        ForecastTime = Convert.ToDateTime(Convert.ToString(y.localDate + "T" + y.timeslot)),
                        temperature = y.temperatureC
                    };
                    forecastGeneralized.Forecasts.Add(item);
                }
            }
            var ObjectFromDatabase = _context.Cities
                .Include(i => i.Forecasts)
                .First(i => i.Name == CityId);
            foreach (var x in forecastGeneralized.Forecasts)
            {
                ObjectFromDatabase.Forecasts.Add(new Forecasts
                {
                    CreatedDate = forecastGeneralized.CreationDate,
                    ForecastTime = x.ForecastTime,
                    Temperature = x.temperature,
                    Provider = forecastGeneralized.Provider,
                });
            }
            _context.SaveChanges();
            return Ok(forecastGeneralized);


        }
        [HttpGet("test/linq/{CityId}/{Provider}")]
        public async Task<ActionResult> TestLinQDatabase(string CityId, string Provider)
        {   
            //Get Unique ID
            //var MySomething = _context.Cities
            //    .Where(i => i.Name == "Vilnius")
            //    .SelectMany(i => i.UniqueProviderID)
            //    .Select(i => new { i.UniqueCityID, i.Provider.ProviderName })
            //    .Where(i => i.ProviderName == "BBC")
            //    .ToList();
            var UniqueProviderID = _context.CityProviderID
                .Where(cp => cp.City.Name == CityId && cp.Provider.ProviderName == Provider)
                .Select(i => new { i.UniqueCityID/*, i.Provider.ProviderName*/ })
                .ToList();
                 return Ok(UniqueProviderID.First().UniqueCityID);
        }

        [HttpGet("test/OWMNow/{CityId}")]
        public async Task<ActionResult> GetTestOWMNow(string CityId)
        {
            var UniqueProviderID = _context.CityProviderID
                .Where(cp => cp.City.Name == CityId && cp.Provider.ProviderName == "OWM")
                .Select(i => new { i.UniqueCityID })
                .ToList();

            if (UniqueProviderID.FirstOrDefault() == null) return NotFound("Not found");
            OWMNowRootObject weatherinfo = await TestAsync<OWMNowRootObject>("OWM", string.Format("weather?{0}&units=metric&appid=4bd458b0d9e2bfadbed92b6b73ce4274", UniqueProviderID.First().UniqueCityID), false);
            ForecastGeneralized forecastGeneralized = new ForecastGeneralized
            {
                Name = CityId,
                CreationDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss")),
                Provider = "OWM",
                Forecasts = new List<ForecastsG>()
            };
            forecastGeneralized= new ForecastGeneralized
            {
                CreationDate=DateTime.Now,
                Name=weatherinfo.name,
                Provider="OWM",
                Forecasts = new List<ForecastsG>
                {
                    new ForecastsG
                    {
                        ForecastTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc) + TimeSpan.FromSeconds(weatherinfo.dt),
                        temperature=weatherinfo.main.temp
                    }
                }
            };
            var ObjectFromDatabase = _context.Cities
                .Include(i => i.ActualTemparture)
                .First(i => i.Name == CityId);
            ObjectFromDatabase.ActualTemparture.Add(new ActualTemperature
            {
                ForecastTime = forecastGeneralized.Forecasts.First().ForecastTime,
                Temperature = forecastGeneralized.Forecasts.First().temperature
            });
            _context.SaveChanges();
            return Ok(forecastGeneralized);

        }
        [HttpGet("test/OWM/{CityId}")]
        public async Task<ActionResult> GetTestOWM(string CityId)
        {
            var UniqueProviderID = _context.CityProviderID
                .Where(cp => cp.City.Name == CityId && cp.Provider.ProviderName == "OWM")
                .Select(i => new { i.UniqueCityID})
                .ToList();

            if (UniqueProviderID.FirstOrDefault() == null) return NotFound("Not found");                    
            OWMOneCallRootObject weatherinfo = await TestAsync<OWMOneCallRootObject>("OWM", string.Format("onecall?{0}&units=metric&appid=4bd458b0d9e2bfadbed92b6b73ce4274", UniqueProviderID.First().UniqueCityID), false);
            ForecastGeneralized forecastGeneralized = new ForecastGeneralized
            {
                Name = CityId,
                CreationDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss")),
                Provider = "OWM",
                Forecasts = new List<ForecastsG>()
            };
           foreach (var x in weatherinfo.hourly)
           {
                ForecastsG item = new ForecastsG
                {
                    temperature = x.temp,
                    ForecastTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)+ TimeSpan.FromSeconds(x.dt),
                };
                item.ForecastTime= Convert.ToDateTime(item.ForecastTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"));
                forecastGeneralized.Forecasts.Add(item);
            }

            var ObjectFromDatabase = _context.Cities
                .Include(i => i.Forecasts)
                .First(i => i.Name == CityId);
            foreach (var x in forecastGeneralized.Forecasts)
            {
                ObjectFromDatabase.Forecasts.Add(new Forecasts
                {
                    CreatedDate = forecastGeneralized.CreationDate,
                    ForecastTime = x.ForecastTime,
                    Temperature = x.temperature,
                    Provider = forecastGeneralized.Provider,
                });
            }
            _context.SaveChanges();
            return Ok(forecastGeneralized);
            
        }
        [HttpGet("test/METEO/{CityId}")]
        public async Task<ActionResult> GetTestMETEO(string CityId)
        {
            var UniqueProviderID = _context.CityProviderID
                .Where(cp => cp.City.Name == CityId && cp.Provider.ProviderName == "METEO")
                .Select(i => new { i.UniqueCityID })
                .ToList();
            if (UniqueProviderID.FirstOrDefault() == null) return NotFound("Not found");

            MeteoRootObject weatherinfo = await TestAsync<MeteoRootObject>("METEO", string.Format("places/{0}/forecasts/long-term", UniqueProviderID.First().UniqueCityID), false);
            ForecastGeneralized forecastGeneralized = new ForecastGeneralized
            {
                Name = weatherinfo.place.name.ToLower(),
                Provider = "Meteo",
                CreationDate = weatherinfo.forecastCreationTimeUtc,
                Forecasts = new List<ForecastsG>()
            };
            foreach (var x in weatherinfo.forecastTimestamps)
            {
                ForecastsG item = new ForecastsG
                {
                    ForecastTime = x.forecastTimeUtc,
                    temperature = x.airTemperature
                };
                forecastGeneralized.Forecasts.Add(item);
            }
            var ObjectFromDatabase = _context.Cities
                .Include(i => i.Forecasts)
                .First(i => i.Name == CityId);
            foreach (var x in forecastGeneralized.Forecasts)
            {
                ObjectFromDatabase.Forecasts.Add(new Forecasts
                {
                    CreatedDate = forecastGeneralized.CreationDate,
                    ForecastTime = x.ForecastTime,
                    Temperature = x.temperature,
                    Provider = forecastGeneralized.Provider,
                });
            }
            _context.SaveChanges();
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
    }
}
