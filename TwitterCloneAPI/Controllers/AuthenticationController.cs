using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using TwitterCloneAPI.Models;
using TwitterCloneAPI.Models.UserResponse;
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
                return BadRequest(new { Message = "User not found" });
            }
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Data!.PasswordHash))
            {
                return BadRequest(new { Message = "Wrong password or username" });
            }
            var token = _tokenService.CreateJwtToken(user.Data);
            user = await _userService.UpdateUserRefreshToken(user.Data);
            if (user.Data is null)
            {
                return StatusCode(500);
            }
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddHours(4),
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Path = "/",
                Secure = true,
                //for Http
                //Domain = Request.Host.Host,
                //Secure = Request.IsHttps
                //SameSite = SameSiteMode.Strict,
            };
            Response.Cookies.Append("JWT", token, cookieOptions);
            string hostUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/";
            return Ok(new UserResponseModel
            {
                UserId = user.Data.UserId,
                UserEmail = user.Data.Email,
                UserName = user.Data.UserProfile!.UserName ?? "Default UserName",
                FullName = user.Data.UserProfile!.FullName ?? "Default FullName",
                ProfilePicture = $"{hostUrl}{user.Data.UserProfile.ProfilePicture!.Replace("\\", "/").Replace("wwwroot/", "")}" ?? "",
                BackPicture = $"{hostUrl}{user.Data.UserProfile.BackPicture!.Replace("\\", "/").Replace("wwwroot/", "")}" ?? "",
                QuantityOfFollowers = user.Data.FollowerUsers.Where(x => x.UserId == user.Data.UserId).Count(),
                QuantityOfFollowing = user.Data.FollowerFollowerUsers.Where(x => x.FollowerUserId == user.Data.UserId).Count(),
                ProfileDescription = user.Data.UserProfile.Bio ?? ""
            });
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            if(string.IsNullOrEmpty(Request.Cookies["JWT"]))
            {
                return Unauthorized();
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(Request.Cookies["JWT"]);
            int localUserId = Convert.ToInt32(token.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
            UserAuthentication user = new();
            if (localUserId <= 0)
            {
                return BadRequest(new { Message = "User not found" });
            }
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
                    SameSite = SameSiteMode.None,
                    Path = "/",
                    Secure = true
                };
                Response.Cookies.Append("JWT", _tokenService.CreateJwtToken(user), cookieOptions);
                return Ok();
            }
            return BadRequest(new { Message = "User not found" });
        }


    }
}
