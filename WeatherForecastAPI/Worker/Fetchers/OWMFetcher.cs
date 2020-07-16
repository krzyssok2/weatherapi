using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using WeatherForecastAPI.Models;
using WeatherForecastAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Xml;

namespace WeatherForecastAPI.Worker
{
    public class OWMFetcher : IFetcher
    {
        public string ProviderName { get; } = "OWM";
        public OWMFetcher(WeatherContext context, IHttpClientFactory httpClientFactory, IServiceScopeFactory scopeFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _scopeFactory = scopeFactory;
        }
        public WeatherContext _context;
        public IHttpClientFactory _httpClientFactory;
        public IServiceScopeFactory _scopeFactory;
        public async Task<ForecastGeneralized> GetDataAsync(string uniqueCityId, string cityName)
        {
            OWMOneCallRootObject weatherinfo = await Deserializer<OWMOneCallRootObject>("OWM", string.Format("onecall?{0}&units=metric&appid=4bd458b0d9e2bfadbed92b6b73ce4274", uniqueCityId), false);
            ForecastGeneralized forecastGeneralized = new ForecastGeneralized
            {
                Name = cityName,
                CreationDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss")),
                Provider = "OWM",
                Forecasts = new List<ForecastsG>()
            };
            foreach (var x in weatherinfo.hourly)
            {
                ForecastsG item = new ForecastsG
                {
                    temperature = x.temp,
                    ForecastTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc) + TimeSpan.FromSeconds(x.dt),
                };
                item.ForecastTime = Convert.ToDateTime(item.ForecastTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"));
                forecastGeneralized.Forecasts.Add(item);
            }
            return forecastGeneralized;
        }


        async Task<T> Deserializer<T>(string provider, string path, bool IsXML)
        {
            var client = _httpClientFactory.CreateClient(provider);
            var result = await client.GetStringAsync(path);
            if (IsXML == true)
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
