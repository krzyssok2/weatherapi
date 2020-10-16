using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using WeatherForecastAPI.Models;
using Newtonsoft.Json;
using System.Net;

namespace WeatherForecastAPI.Worker
{
    public class BBCFetcher:IFetcher
    {
        public string ProviderName { get; } = "BBC";
        public BBCFetcher(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public IHttpClientFactory _httpClientFactory;

        public async Task<ForecastGeneralized> GetData(string uniqueCityId, string cityName)
        {
            using WebClient client = new WebClient();
            string doc = client.DownloadString(Uri.EscapeUriString(string.Format("https://www.bbc.com/weather/{0}?day=1/application/json", uniqueCityId)));
            doc = doc.Substring(doc.IndexOf("application/json"));
            doc = doc.Substring(doc.IndexOf("{"));
            doc = doc.Substring(0, doc.IndexOf("<"));
            BBCScrapRootObject weatherinfo = JsonConvert.DeserializeObject<BBCScrapRootObject>(doc);
            ForecastGeneralized forecastGeneralized = new ForecastGeneralized
            {
                Name = weatherinfo.data.location.name.ToLower(),
                Provider = "BBC",
                CreationDate = DateTime.Now,
                Forecasts = new List<ForecastsG>()
            };
            foreach (var day in weatherinfo.data.forecasts.Take(7))
            {
                foreach (var forecast in day.detailed.reports)
                {
                    ForecastsG item = new ForecastsG
                    {
                        ForecastTime = Convert.ToDateTime(Convert.ToString(forecast.localDate + "T" + forecast.timeslot)),
                        temperature = forecast.temperatureC
                    };
                    forecastGeneralized.Forecasts.Add(item);
                }
            }
            return await Task.FromResult(forecastGeneralized);
        }
    }
}
