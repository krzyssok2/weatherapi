using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherForecastAPI.Models;

namespace WeatherForecastAPI.Entities
{
    public class FavoriteCities
    {
        public long Id { get; set; }

        public long UserId { get; set; }
        public UserSettings User { get; set; }
        public Cities City { get; set; }
    }
}
