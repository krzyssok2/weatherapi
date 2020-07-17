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

        public async Task<ForecastGeneralized> GetDataAsync(string uniqueCityId, string cityName)
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
                CreationDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss")),
                Forecasts = new List<ForecastsG>()
            };
            foreach (var x in weatherinfo.data.forecasts.Take(7))
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
            return forecastGeneralized;
        }
    }
}
