using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeatherForecastAPI.Entities;

namespace WeatherForecastAPI.EntityConfiguration
{
    public class CitiesConfiguration : IEntityTypeConfiguration<Cities>
    {
        public void Configure(EntityTypeBuilder<Cities> builder)
        {
            builder.HasMany(i => i.UniqueProviderID)
                .WithOne(i => i.City);
        }
    }
}
