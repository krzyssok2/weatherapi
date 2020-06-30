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
    }
    public class UsersSettings
    {
        public Temperature PrefferedUnit{ get;set; }
        public List<PrefferedCities> PrefferedCities { get; set; }
    }
    public class PrefferedCities
    {
        public string CityName { get; set; }
        public string Country { get; set; }
    }
    public class PreferedTemperature
    {
        public Temperature PrefferedUnit { get; set; }
    }
}
