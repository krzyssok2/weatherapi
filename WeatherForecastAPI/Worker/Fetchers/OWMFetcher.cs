using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using WeatherForecastAPI.Models;
using Newtonsoft.Json;

namespace WeatherForecastAPI.Worker
{
    public class OWMFetcher :IFetcher
    {
        public string ProviderName { get; } = "OWM";
        public OWMFetcher(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public IHttpClientFactory _httpClientFactory;
        public async Task<ForecastGeneralized> GetData(string uniqueCityId, string cityName)
        {
            OWMOneCallRootObject weatherinfo = await Deserialize<OWMOneCallRootObject>("OWM", string.Format("onecall?{0}&units=metric&appid=4bd458b0d9e2bfadbed92b6b73ce4274", uniqueCityId), false);
            ForecastGeneralized forecastGeneralized = new ForecastGeneralized
            {
                Name = cityName,
                CreationDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss")),
                Provider = "OWM",
                Forecasts = new List<ForecastsG>()
            };
            foreach (var forecast in weatherinfo.hourly)
            {
                ForecastsG item = new ForecastsG
                {
                    temperature = forecast.temp,
                    ForecastTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc) + TimeSpan.FromSeconds(forecast.dt),
                };
                item.ForecastTime = Convert.ToDateTime(item.ForecastTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"));
                forecastGeneralized.Forecasts.Add(item);
            }
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
