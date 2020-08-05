using WeatherForecastAPI.Models.Swagger;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using WeatherForecastAPI.Entities;
using WeatherForecastAPI.Options;
using WeatherForecastAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WeatherForecastAPI.Services;

namespace WeatherForecastAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly WeatherContext _context;
        private readonly AuthServices service;
        public AuthController(UserManager<IdentityUser> identityService, SignInManager<IdentityUser> signInManager, 
            JwtSettings jwtSettings,TokenValidationParameters tokenValidationParameters, WeatherContext weatherContext)
        {
            _userManager = identityService;
            _jwtSettings = jwtSettings;
            _tokenValidationParameters = tokenValidationParameters;
            _context = weatherContext;
            service = new AuthServices(_userManager, _jwtSettings, _tokenValidationParameters, _context);
        }
        /// <summary>
        /// Log in user
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Successfully logged in</response>   
        /// <response code="400">Wasn't able to log in</response>   
        [HttpPost("Login")]
        [ProducesResponseType(typeof(AuthSuccessResponse), 200)]
        [ProducesResponseType(typeof(LogInErrorsResponse), 401)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<ActionResult> LoginAsync([FromBody] AuthAccount request)
        {
            var authResponse = await service.LogIn(request.Email, request.Password);

            if (!authResponse.Success)
            {
                return Unauthorized(new AuthFailedResponse
                { 
                    Error = authResponse.Error
                });
            }
            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token,
                RefreshToken = authResponse.RefreshToken
            });
        }
        /// <summary>
        /// Refresh user token
        /// </summary>
        /// <returns></returns>
        [HttpPost("Refresh")]
        [ProducesResponseType(typeof(AuthSuccessResponse), 200)]
        [ProducesResponseType(typeof(TokenErrorsResponse), 400)]
        public async Task<ActionResult> RefreshAsync([FromBody] RefreshTokenRequest request)
        {
            var authResponse = await service.RefreshTokenAsync(request.Token, request.RefreshToken);

            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Error = authResponse.Error
                });
            }

            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token,
                RefreshToken = authResponse.RefreshToken
            });
        }
        /// <summary>
        /// Register user
        /// </summary>
        /// <returns></returns>
        [HttpPost("Registration")]
        [ProducesResponseType(typeof(AuthSuccessResponse), 200)]
        [ProducesResponseType(typeof(IdentityError), 400)]
        public async Task<ActionResult> RegisterAsync([FromBody] AuthAccount request)
        {
            var result = await _userManager.CreateAsync(new IdentityUser
            {
                UserName = request.Email,
                Email = request.Email,
            }, request.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new 
                {
                    result.Errors,
                });
            }
            else
            {
                _context.UserSettings.Add(new UserSettings
                {
                    User = request.Email,
                    Units = 0,
                });
                _context.SaveChanges();
            }
            return Ok();
        }
    }
}