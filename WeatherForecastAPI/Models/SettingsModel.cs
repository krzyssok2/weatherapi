using System.Collections.Generic;

namespace WeatherForecastAPI.Models
{
    public enum Temperature
    {
        //Starting from 0 breaks FluentValidator, can't set C then
        C=0,
        F=1,
        K=2,
        Ra=3,
        Re=4,
    }
    public class UserAccount
    {
        public string Username { get; set; }
        public long UserId { get; set; }
        public Settings UserSettings { get; set; }
    }
    public class Settings
    {
        public Temperature PreferedUnit { get; set; }
        public List<PreferedCities> FavoriteCities { get; set; }
    }
    public class AllPreferedCities
    {
        public List<PreferedCities> PreferedCities { get; set; }
    }
    public class DeleteCity
    {
        /// <example>vilnius</example>
        public string Cityname { get; set; }
        /// <example>lithuania</example>
        public string Country { get; set; }
    }
    public class PreferedCities
    {
        /// <example>1</example>
        public long CityId { get; set; }
        /// <example>vilnius</example>
        public string CityName { get; set; }
        /// <example>lithuania</example>
        public string Country { get; set; }
    }
    public class InsertCities
    {
        public ICollection<CitiesId> CitiesId {get;set;}
    }
    public class InsertSettings
    {
        public Temperature Units { get; set; }
        public ICollection<CitiesId> CitiesId { get; set; }
    }
    public class CitiesId
    {
        /// <example>1</example>
        public long CityId { get; set; }
    }
    public class PreferedTemperature
    {
        public Temperature PrefferedUnit { get; set; }
    }
}
