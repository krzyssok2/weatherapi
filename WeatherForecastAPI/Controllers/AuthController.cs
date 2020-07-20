using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<IdentityUser> _identityService;

        public AuthController(UserManager<IdentityUser> identityService)
        {
            _identityService = identityService;
        }
        /// <summary>
        /// Registration
        /// </summary>
        /// <returns></returns>
        [HttpPost("Login")]
        public ActionResult<AuthAccount> GetUserAccountInfo()
        {
            AuthAccount userAccount = new AuthAccount();

            return Ok();
        }
        /// <summary>
        /// Login authentication
        /// </summary>
        /// <returns></returns>
        [HttpPost("Registration")]
        public async Task<ActionResult> RegisterAsync([FromBody] AuthAccount request)
        {
            var result = await _identityService.CreateAsync(new IdentityUser
            {
                UserName = request.Email,
                Email = request.Email,
            }, request.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new AuthFailedResponse()
                {
                    Errors = result.Errors
                });
            }

            return Ok();
        }
    }
}
