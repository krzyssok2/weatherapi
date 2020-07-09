using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherForecastAPI.Entities;

namespace WeatherForecastAPI.EntityConfiguration
{
    public class UniqueProviderIDConfiguration: IEntityTypeConfiguration<UniqueProviderID>
    {
        public void Configure(EntityTypeBuilder<UniqueProviderID> builder)
        {
            builder.HasOne(i => i.City)
                .WithMany(i => i.UniqueProviderID);

            builder.HasOne(i => i.Provider)
                .WithMany(i => i.UniqueID);
        }
    }
}
