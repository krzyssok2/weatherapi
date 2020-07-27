using System.Collections.Generic;

namespace WeatherForecastAPI.Models
{
    public enum Temperature
    {
        C=0,
        F=1,
        K=2
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
        public string Cityname { get; set; }
        public string Country { get; set; }
    }
    public class PreferedCities
    {
        public long CityId { get; set; }
        public string CityName { get; set; }
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
        public long CityId { get; set; }
    }
    public class PreferedTemperature
    {
        public Temperature PrefferedUnit { get; set; }
    }
}
