using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherForecastAPI.Entities
{
    public class Providers
    {
        public long Id { get; set; }
        public string ProviderName { get; set; }
        public ICollection<UniqueProviderID> UniqueID { get; set; }
    }
}
