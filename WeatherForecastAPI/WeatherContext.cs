using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherForecastAPI.Entities;
using WeatherForecastAPI.EntityConfiguration;

namespace WeatherForecastAPI
{
    public class WeatherContext : DbContext
    {
        public WeatherContext(DbContextOptions options): base(options) { }

        public DbSet<Cities> Cities { get; set; }
        public DbSet<Forecasts> Forecasts { get; set; }
        public DbSet<Providers> Providers { get; set; }
        public DbSet<UniqueProviderID> CityProviderID { get; set; }
        public DbSet<ActualTemperature> ActualTemperatures { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new CitiesConfiguration());
            builder.ApplyConfiguration(new ProvidersConfiguration());
            builder.ApplyConfiguration(new UniqueProviderIDConfiguration());
        }
    }
}
