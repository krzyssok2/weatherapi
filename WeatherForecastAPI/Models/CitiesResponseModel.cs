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
        /// <example>1</example>
        public long CityId { get; set; }
        /// <example>vilnius</example>
        public string CityName { get;set; }
        /// <example>lithuania</example>
        public string Country { get; set; }
        public DateTime FromDate { get; set; } 
        public DateTime ToDate { get; set; }
    }
}
