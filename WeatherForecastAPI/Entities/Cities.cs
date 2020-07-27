using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WeatherForecastAPI.Models;

namespace WeatherForecastAPI.Entities
{
    public class Cities
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public ICollection<Forecasts> Forecasts { get; set; }
        public ICollection<UniqueProviderID> UniqueProviderID { get; set; }
        public ICollection<ActualTemperature> ActualTemparture { get; set; }
        public ICollection<FavoriteCities> FavoritedByUser { get; set; }
    }
}
