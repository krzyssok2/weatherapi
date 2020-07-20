using System;
using System.Collections.Generic;
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
        public DateTime FromDate { get; set; } 
        public DateTime ToDate { get; set; }
    }
}
