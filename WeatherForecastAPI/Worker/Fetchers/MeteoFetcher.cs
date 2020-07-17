using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using WeatherForecastAPI.Models;
using Newtonsoft.Json;
using System.Xml;

namespace WeatherForecastAPI.Worker
{
    public class MeteoFetcher : IFetcher
    {
        public string ProviderName { get; } = "Meteo";
        public MeteoFetcher(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public IHttpClientFactory _httpClientFactory;
        public async Task<ForecastGeneralized> GetDataAsync(string uniqueCityId,string cityName)
        {
            MeteoRootObject weatherinfo =
                await Deserializer<MeteoRootObject>("METEO", string.Format("places/{0}/forecasts/long-term", uniqueCityId), false);
            ForecastGeneralized forecastGeneralized = new ForecastGeneralized
            {
                Name = weatherinfo.place.name.ToLower(),
                Provider = "Meteo",
                CreationDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss")),
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
