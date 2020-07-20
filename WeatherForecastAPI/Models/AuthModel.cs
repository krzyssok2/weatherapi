using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherForecastAPI.Models
{
    public class AuthAccount
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class AuthenticationResult
    {
        public string Token { get; set; }
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
    public class AuthSuccessResponse
    {
        public string Token { get; set; }
    }
    public class AuthFailedResponse
    {
        //TODO: FIX THIS
        public IEnumerable<IdentityError> Errors { get; set; }
    }
}
