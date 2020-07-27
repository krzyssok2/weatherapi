using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WeatherForecastAPI.Entities;
using WeatherForecastAPI.EntityConfiguration;

namespace WeatherForecastAPI
{
    public class WeatherContext : IdentityDbContext<IdentityUser>
    {
        public WeatherContext(DbContextOptions options): base(options) { }

        public DbSet<Cities> Cities { get; set; }
        public DbSet<Forecasts> Forecasts { get; set; }
        public DbSet<Providers> Providers { get; set; }
        public DbSet<UniqueProviderID> CityProviderID { get; set; }
        public DbSet<ActualTemperature> ActualTemperatures { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<UserSettings> UserSettings { get; set; }
        public DbSet<FavoriteCities> FavoriteCities { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new CitiesConfiguration());
            builder.ApplyConfiguration(new ProvidersConfiguration());
            builder.ApplyConfiguration(new UniqueProviderIDConfiguration());
            builder.ApplyConfiguration(new FavoriteCitiesConfiguration());
        }
    }
}
