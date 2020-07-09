using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WeatherForecastAPI.Models;

namespace WeatherForecastAPI.Entities
{
    public class UniqueProviderID
    {
        public long Id { get; set; }
        public Cities City { get; set; }
        public string UniqueCityID { get; set; }
        public Providers Provider { get; set; }
    }
}
