using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherForecastAPI.Models
{
    public class CitiesResponseModel
    {
        public List<CityInfoResponseModel> Cities { get; set; }        
    }

    public class CityInfoResponseModel
    {
        public long CityId { get; set; }
        public string CityName { get;set; }
        public string Country { get; set; }
    }
}
