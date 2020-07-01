using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherForecastAPI.Models;

namespace WeatherForecastAPI.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// Registration
        /// </summary>
        /// <returns></returns>
        [HttpPost("Registration")]
        public ActionResult<AuthAccount> GetUserAccountInfo()
        {
            AuthAccount UserAccount = new AuthAccount();

            return Ok();
        }
        /// <summary>
        /// Login authentication
        /// </summary>
        /// <returns></returns>
        [HttpPost("Login")]
        public ActionResult<AuthToken> CreateUserAccount()
        {
            AuthToken Token = new AuthToken();

            return Token;
        }
    }
}
