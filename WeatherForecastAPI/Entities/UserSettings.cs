using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherForecastAPI.Models;

namespace WeatherForecastAPI.Entities
{
    public class UserSettings
    {
        public long Id { get; set; }
        public string User { get; set; }
        public Temperature Units { get; set; }
        public ICollection<FavoriteCities> FavoriteCities { get; set; }
    }
}
