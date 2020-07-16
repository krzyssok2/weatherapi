using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WeatherForecastAPI.Models;
using WeatherForecastAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Xml;
using System.Net;

namespace WeatherForecastAPI.Worker
{
    public class FetcherService : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory scopeFactory;
        private Timer _timer;
        public FetcherService(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoStuff, null, TimeSpan.Zero,
                TimeSpan.FromHours(1));
            return Task.CompletedTask;
        }


        void DoStuff(object body)
        {
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<WeatherContext>();
            if(context.Forecasts.Max(i=> i.CreatedDate).Hour!=DateTime.Now.Hour)
            {
                List<IFetcher> Services = new List<IFetcher>
                {
                    scope.ServiceProvider.GetRequiredService<BBCFetcher>(),
                    scope.ServiceProvider.GetRequiredService<MeteoFetcher>(),
                    scope.ServiceProvider.GetRequiredService<OWMFetcher>(),
                };
                IFetcher FactualService = scope.ServiceProvider.GetRequiredService<OWMActualFetcher>();
                foreach (var city in context.Cities.ToList())
                {
                    foreach (var service in Services)
                    {
                        var uniqueCityId = GetUniqueID(city.Name, service.ProviderName, context);
                        if (uniqueCityId == null) continue;
                        var generalized = service.GetDataAsync(uniqueCityId, city.Name).Result;
                        DBUpdateForecasts(generalized, city.Name, context);
                    }
                }
                foreach (var city in context.Cities.ToList())
                {
                    var uniqueCityId = GetUniqueID(city.Name, "OWM", context);
                    if (uniqueCityId == null) continue;
                    var generalized = FactualService.GetDataAsync(uniqueCityId, city.Name).Result;
                    DbUpdateActual(generalized, city.Name, context);
                }
            }
        }
        void DbUpdateActual(ForecastGeneralized generalized,string cityId, WeatherContext _context)
        {
            var ObjectFromDatabase = _context.Cities
                .Include(i => i.ActualTemparture)
                .First(i => i.Name == cityId);
            ObjectFromDatabase.ActualTemparture.Add(new ActualTemperature
            {
                ForecastTime = generalized.Forecasts.First().ForecastTime,
                Temperature = generalized.Forecasts.First().temperature
            });
            _context.SaveChanges();
        }
        string GetUniqueID(string CityId, string Provider, WeatherContext _context)
        {
            var UniqueProviderID = _context.CityProviderID
                .Where(cp => cp.City.Name == CityId && cp.Provider.ProviderName == Provider)
                .Select(UnqiueID => new { UnqiueID.UniqueCityID })
                .ToList();
            if (UniqueProviderID.FirstOrDefault() == null)
            {
                return "Not Found";
            }
            else
            {
                return UniqueProviderID.First().UniqueCityID;
            }
        }
        void DBUpdateForecasts(ForecastGeneralized forecastGeneralized, string CityId, WeatherContext _context)
        {
            var ObjectFromDatabase = _context.Cities
                .Include(city => city.Forecasts)
                .First(city => city.Name == CityId);
            foreach (var forecast in forecastGeneralized.Forecasts)
            {
                ObjectFromDatabase.Forecasts.Add(new Forecasts
                {
                    CreatedDate = forecastGeneralized.CreationDate,
                    ForecastTime = forecast.ForecastTime,
                    Temperature = forecast.temperature,
                    Provider = forecastGeneralized.Provider,
                });
            }
            _context.SaveChanges();
        }
        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }


    }
}
