using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherForecastAPI.Entities;

namespace WeatherForecastAPI.EntityConfiguration
{
    public class ProvidersConfiguration : IEntityTypeConfiguration<Providers>
    {
        public void Configure(EntityTypeBuilder<Providers> builder)
        {
            builder.HasMany(i => i.UniqueID)
                .WithOne(i => i.Provider);
        }
    }
}
