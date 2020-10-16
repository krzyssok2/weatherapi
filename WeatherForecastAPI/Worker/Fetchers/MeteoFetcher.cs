using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using WeatherForecastAPI.Models;
using Newtonsoft.Json;

namespace WeatherForecastAPI.Worker
{
    public class MeteoFetcher : IFetcher
    {
        public string ProviderName { get; } = "METEO";
        public MeteoFetcher(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public IHttpClientFactory _httpClientFactory;
        public async Task<ForecastGeneralized> GetData(string uniqueCityId,string cityName)
        {
            MeteoRootObject weatherinfo =
                await Deserialize<MeteoRootObject>("METEO", string.Format("places/{0}/forecasts/long-term", uniqueCityId));
            ForecastGeneralized forecastGeneralized = new ForecastGeneralized
            {
                Name = weatherinfo.place.name.ToLower(),
                Provider = "Meteo",
                CreationDate = DateTime.Now,
                Forecasts = new List<ForecastsG>()
            };
            foreach (var forecast in weatherinfo.forecastTimestamps)
            {
                ForecastsG item = new ForecastsG
                {
                    ForecastTime = forecast.forecastTimeUtc,
                    temperature = forecast.airTemperature
                };
                forecastGeneralized.Forecasts.Add(item);
            }
            return forecastGeneralized;
        }

        async Task<T> Deserialize<T>(string provider, string path)
        {
            var client = _httpClientFactory.CreateClient(provider);
            var result = await client.GetStringAsync(path);
            T myClass = JsonConvert.DeserializeObject<T>(result);
            return myClass;
        }
    }
}
