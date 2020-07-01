using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherForecastAPI.Models
{
    public class AuthAccount
    {
        public string Gmail { get; set; }
        public string Password { get; set; }
    }
    public class AuthToken
    {
        public string Token { get; set; }
    }
}
