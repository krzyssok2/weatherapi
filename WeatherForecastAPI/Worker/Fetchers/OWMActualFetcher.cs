using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using WeatherForecastAPI.Models;
using Newtonsoft.Json;
using System.Xml;

namespace WeatherForecastAPI.Worker
{
    public class OWMActualFetcher 
    {
        public string ProviderName { get; } = "OWM";
        public OWMActualFetcher(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public IHttpClientFactory _httpClientFactory;
        public async Task<ForecastGeneralized> GetDataAsync(string uniqueCityId, string cityName)
        {
            OWMNowRootObject weatherinfo = await Deserializer<OWMNowRootObject>("OWM", string.Format("weather?{0}&units=metric&appid=4bd458b0d9e2bfadbed92b6b73ce4274", uniqueCityId), false);
            ForecastGeneralized forecastGeneralized = new ForecastGeneralized
            {
                Name = cityName,
                CreationDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss")),
                Provider = "OWM",
                Forecasts = new List<ForecastsG>()
            };
            forecastGeneralized = new ForecastGeneralized
            {
                CreationDate = DateTime.Now,
                Name = weatherinfo.name,
                Provider = "OWM",
                Forecasts = new List<ForecastsG>
                {
                    new ForecastsG
                    {
                        ForecastTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc) + TimeSpan.FromSeconds(weatherinfo.dt),
                        temperature=weatherinfo.main.temp
                    }
                }
            };
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
