using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TwitterCloneAPI.Models;
using TwitterCloneAPI.Models.UserRequest;
using TwitterCloneAPI.Services.Token;
using TwitterCloneAPI.Services.User;

namespace TwitterCloneAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthenticationController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("Registration")]
        [AllowAnonymous]
        public async Task<IActionResult> Registration(UserRequestModel request)
        {
            var response = await _userService.CreateUser(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);

        }

        [HttpPost("Authorization")]

        public async Task<IActionResult> Authorization(UserRequestModel request)
        {
            var user = await _userService.GetUserByEmail(request);

            if (user.Data == null)
            {
                return BadRequest("User not found");
            }
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Data!.PasswordHash))
            {
                return BadRequest("Wrong password or username");
            }
            var token = _tokenService.CreateJwtToken(user.Data);
            user = await _userService.UpdateUserRefreshToken(user.Data);
            if (user.Data is null)
            {
                return StatusCode(500);
            }
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
            return Ok(user.Data.UserProfile);//TODO: return custom object
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(Request.Cookies["JWT"]);
            int localUserId = Convert.ToInt32(token.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
            UserAuthentication user = new();
            if (localUserId <= 0)
            {
                return BadRequest("User not found");
            }
            else
            {
                user = _userService.GetUserById(localUserId).Result.Data!;
                if (user is not null)
                {
                    if (user.TokenExpires < DateTime.Now)
                    {
                        return Unauthorized();
                    }
                    var updatedUser = await _userService.UpdateUserRefreshToken(user);
                    if (updatedUser.Data is null)
                    {
                        return StatusCode(500);
                    }
                    var cookieOptions = new CookieOptions
                    {
                        Expires = DateTime.Now.AddHours(4),
                        HttpOnly = true,
                        Domain = Request.Host.Host,
                        SameSite = SameSiteMode.Strict,
                        Path = "/",
                        Secure = true
                    };
                    Response.Cookies.Append("JWT", _tokenService.CreateJwtToken(user), cookieOptions);
                    return Ok();
                }
                return BadRequest("User not found");
            }


        }
    }
}
