using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherForecastAPI.Models
{
    public enum Temperature
    {
        C,
        F,
        K
    }
    public class AllUsers
    {
        public List<UserAccount> UserAccounts { get; set; }
    }
    public class UserAccount
    {
        public string Username { get; set; }
        public long UserId { get; set; }
        public UsersSettings UserSettings { get; set; }
    }
    public class UsersSettings
    {
        public Temperature PreferedUnit{ get;set; }
        public List<PreferedCities> FavoriteCities { get; set; }
    }
    public class PreferedCities
    {
        public long CityId { get; set; }
        public string CityName { get; set; }
        public string Country { get; set; }
    }
    public class PreferedTemperature
    {
        public Temperature PrefferedUnit { get; set; }
    }
}
