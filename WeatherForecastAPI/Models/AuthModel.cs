using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherForecastAPI.Models
{
    public enum Errors
    {
        InvalidToken,
        TokenExpired,
        TokenDoesntExist,
        RefreshTokenExpired,
        RefreshTokenInvalid,
        RefreshTokenUsed,
        RefreshTokenDoesntMatchJwT,
        UserDoesntExist,
        PassNickWrong
    }
    public class AuthAccount
    {
        /// <example>email@gmail.com</example>
        public string Email { get; set; }
        /// <example>Password567!</example>
        public string Password { get; set; }
    }
    public class AuthenticationResult
    {
        public AuthenticationResult(Errors error)
        {
            Error = error;
        }
        public AuthenticationResult()
        {
        }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public bool Success { get; set; }
        public Errors Error { get; set; }
    }
    public class AuthSuccessResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
    public class RefreshTokenRequest
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
    public class AuthFailedResponse
    {
        public Errors Error { get; set; }
    }
}
