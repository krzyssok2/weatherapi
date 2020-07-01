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
        [HttpGet]
        public ActionResult<AuthAccount> GetUserAccountInfo()
        {
            AuthAccount UserAccount = new AuthAccount();

            return UserAccount;
        }
        [HttpPost]
        public ActionResult<AuthAccount> CreateUserAccount()
        {
            AuthAccount UserAccount = new AuthAccount();

            return UserAccount;
        }
        [HttpPut]
        public ActionResult<AuthAccount> UpdateUserAccount()
        {
            AuthAccount UserAccount = new AuthAccount();

            return UserAccount;
        }
    }
}
