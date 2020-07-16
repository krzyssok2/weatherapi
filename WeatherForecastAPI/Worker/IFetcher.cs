using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherForecastAPI.Models;

namespace WeatherForecastAPI
{
    interface IFetcher
    {
        string ProviderName { get; }
        Task<ForecastGeneralized> GetDataAsync(string uniqueCityId, string cityName);
    }
}
