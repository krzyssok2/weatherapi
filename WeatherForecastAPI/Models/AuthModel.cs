using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherForecastAPI.Models
{
    public enum ErrorsList
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
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class AuthenticationResult
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public bool Success { get; set; }
        public ErrorsList Errors { get; set; }
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
        public ErrorsList Errors { get; set; }
    }
}
