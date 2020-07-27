using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using WeatherForecastAPI.Models;
using Newtonsoft.Json;
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
        public async Task<ForecastGeneralized> GetData(string uniqueCityId, string cityName)
        {
            OWMNowRootObject weatherinfo = await Deserialize<OWMNowRootObject>("OWM", string.Format("weather?{0}&units=metric&appid=4bd458b0d9e2bfadbed92b6b73ce4274", uniqueCityId), false);
            ForecastGeneralized forecastGeneralized = new ForecastGeneralized
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
        async Task<T> Deserialize<T>(string provider, string path, bool IsXML)
        {
            var client = _httpClientFactory.CreateClient(provider);
            var result = await client.GetStringAsync(path);
            T myClass = JsonConvert.DeserializeObject<T>(result);
            return myClass;
        }
    }
}
