using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherForecastAPI.Models
{
    public class AllUsers
    {
        public List<UserAccount> UserAccounts { get; set; }
    }
    public class UserAccount
    {
        public long UserId { get; set; }
        public UsersSettings UserSettings {get;set;}
    }
    public class UsersSettings
    {
        public bool PreferanceC { get; set; }
        public List<PrefferedCities> PrefferedCities { get; set; }
    }
    public class PrefferedCities
    {
        public string CityName { get; set; }
        public string Country { get; set; }
    }
}
