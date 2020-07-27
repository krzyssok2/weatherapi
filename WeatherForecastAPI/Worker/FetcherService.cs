using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WeatherForecastAPI.Models;
using WeatherForecastAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Serilog;

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
            _timer = new Timer(DoStuff, null, TimeSpan.Zero,TimeSpan.FromHours(1));
            return Task.CompletedTask;
        }

        void DoStuff(object body)
        {
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<WeatherContext>();
            if (WasWeatherAlreadyFetced(context)) return;

            var services = scope.ServiceProvider.GetRequiredService<List<IFetcher>>();
            var uniqueId = context.CityProviderID
                .Include(i=>i.City)
                .Include(i=>i.Provider)
                .AsNoTracking().ToList();
            foreach (var city in context.Cities.ToList())
            {
                foreach (var service in services)
                {
                    string uniqueCityId = uniqueId.FirstOrDefault(cp => cp.City.Name == city.Name && cp.Provider.ProviderName == service.ProviderName).UniqueCityID;
                    if (uniqueCityId == null)
                    {
                        Log.Error($"Failed to find unqiue city id from provider:{service.ProviderName} for city: {city.Name}");
                        continue;
                    }
                    try
                    {
                        var generalized = service.GetData(uniqueCityId, city.Name).Result;
                        DBUpdateForecasts(generalized, city.Name, context);
                        Log.Information($"Succesfully fetched data from provider:{service.ProviderName} for city: {city.Name}");
                    }
                    catch
                    {
                        Log.Error($"Failed to fetch data from provider: {service.ProviderName} for city:{city.Name}");
                    }
                }
            }

            OWMActualFetcher factualService = scope.ServiceProvider.GetRequiredService<OWMActualFetcher>();
            foreach (var city in context.Cities.ToList())
            {
                var uniqueCityId = uniqueId.FirstOrDefault(cp => cp.City.Name == city.Name && cp.Provider.ProviderName == "OWM").UniqueCityID;
                if (uniqueCityId == null)
                {
                    Log.Error($"Failed to get factual data for {city.Name}");
                    continue;
                }
                try
                {
                    Log.Information($"Successfully fetched factual data from {city.Name}");
                    var generalized = factualService.GetData(uniqueCityId, city.Name).Result;
                    DbUpdateActual(generalized, city.Name, context);
                }
                catch
                {
                    Log.Error($"Failed to get factual data for {city.Name}");
                }
            }
        }

        bool WasWeatherAlreadyFetced(WeatherContext context)
        {
            return context.Forecasts.Max(i => i.CreatedDate).Hour == DateTime.Now.Hour;
        }
        void DbUpdateActual(ForecastGeneralized generalized,string cityId, WeatherContext _context)
        {
            var objectFromDatabase = _context.Cities
                .Include(i => i.ActualTemparture)
                .First(i => i.Name == cityId);
            objectFromDatabase.ActualTemparture.Add(new ActualTemperature
            {
                ForecastTime = generalized.Forecasts.First().ForecastTime,
                Temperature = generalized.Forecasts.First().temperature
            });
            _context.SaveChanges();
        }
        void DBUpdateForecasts(ForecastGeneralized forecastGeneralized, string CityId, WeatherContext _context)
        {
            var objectFromDatabase = _context.Cities
                .Include(city => city.Forecasts)
                .First(city => city.Name == CityId);
            foreach (var forecast in forecastGeneralized.Forecasts)
            {
                objectFromDatabase.Forecasts.Add(new Forecasts
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
