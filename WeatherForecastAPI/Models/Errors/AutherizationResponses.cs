using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherForecastAPI.Models.Swagger
{ 
    public enum ErrorsTokens
    {
        InvalidToken,
        TokenExpired,
        TokenDoesntExist,
        RefreshTokenExpired,
        RefreshTokenInvalid,
        RefreshTokenUsed,
        RefreshTokenDoesntMatchJwT,
    }
    public enum ErrorsAccount
    {
        UserDoesntExist,
        PassNickWrong
    }

    public class LogInErrorsResponse
    {
        public ErrorsAccount Error { get; set; }
    }
    public class TokenErrorsResponse
    {
        public ErrorsTokens Error { get; set; }
    }

}
