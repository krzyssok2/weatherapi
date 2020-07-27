using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherForecastAPI.Entities;
using WeatherForecastAPI.Models;

namespace WeatherForecastAPI.EntityConfiguration
{
    public class FavoriteCitiesConfiguration : IEntityTypeConfiguration<FavoriteCities>
    {
        public void Configure(EntityTypeBuilder<FavoriteCities> builder)
        {
            builder.HasOne(i => i.City)
                .WithMany(i => i.FavoritedByUser);

            builder.HasOne(i => i.User)
                .WithMany(i => i.FavoriteCities)
                .HasForeignKey(x => x.UserId);
        }
    }
}
