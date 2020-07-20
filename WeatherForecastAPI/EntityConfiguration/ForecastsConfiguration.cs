using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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
