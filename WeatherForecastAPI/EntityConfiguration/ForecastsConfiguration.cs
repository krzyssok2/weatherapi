using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherForecastAPI.Entities;

namespace WeatherForecastAPI.EntityConfiguration
{
    public class ForecastsConfiguration: IEntityTypeConfiguration<Forecasts>
    {
        public void Configure(EntityTypeBuilder<Forecasts> builder)
        {
            
        }
    }
}
