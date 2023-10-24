using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwitterCloneAPI.Models.UserRequest;
using TwitterCloneAPI.Services.Token;
using TwitterCloneAPI.Services.User;

namespace TwitterCloneAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthenticationController(IConfiguration configuration, IUserService userService, ITokenService tokenService)
        {
            _configuration = configuration;
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("Registration")]
        [AllowAnonymous]
        public async Task<IActionResult> Registration(UserRequestModel request)
        {
            var response = await _userService.CreateUser(request);
            if(!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
            
        }

        [HttpPost("Authorization")]

        public async Task<IActionResult> Authorization(UserRequestModel request)
        {
            var user = await _userService.GetUserByEmail(request);

            if(user.Data == null)
            {
                return BadRequest("User not found");
            }
            if(!BCrypt.Net.BCrypt.Verify(request.Password, user.Data!.PasswordHash))
            {
                return BadRequest("Wrong password or username");
            }
            var token = _tokenService.CreateJwtToken(user.Data);

            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddHours(4),
                HttpOnly = true,
                Domain = Request.Host.Host,
                SameSite = SameSiteMode.Strict,
                Path = "/",
                Secure = true
            };
            Response.Cookies.Append("JWT", token, cookieOptions);
            return Ok(user);
        }

    }
}
