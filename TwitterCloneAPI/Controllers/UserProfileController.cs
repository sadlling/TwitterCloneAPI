using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;
using TwitterCloneAPI.Models;
using TwitterCloneAPI.Models.UserProfileRequest;
using TwitterCloneAPI.Services.UserProfiles;

namespace TwitterCloneAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;

        public UserProfileController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        [HttpGet("GetUserProfileById{userId}")]
        public async Task<IActionResult> GetProfileByUserId(int userId)
        {
            var response = await _userProfileService.GetProfileByUserId(userId);
            string hostUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/";
            if (response.Data is not null)
            {
                if (!string.IsNullOrEmpty(response.Data.ProfilePicture))
                    response.Data.ProfilePicture = $"{hostUrl}{response.Data.ProfilePicture}";
                if (!string.IsNullOrEmpty(response.Data.BackPicture))
                    response.Data.BackPicture = $"{hostUrl}{response.Data.BackPicture}";
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("GetCurrentUserProfile")]
        public async Task<IActionResult> GetCurrentUserProfile()
        {
            if (Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)) <= 0)
            {
                return Unauthorized();
            }
            var response = await _userProfileService.GetCurrentUserProfile(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            string hostUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/";
            if (response.Data is not null)
            {
                response.Data.ProfilePicture = $"{hostUrl}{response.Data.ProfilePicture}";
                response.Data.BackPicture = $"{hostUrl}{response.Data.BackPicture}";
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpPut("UpdateUserProfile")]
        public async Task<IActionResult> UpdateUserProfile([FromForm] UpdateUserProfileRequest profile)
        {
            if (Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)) <= 0)
            {
                return Unauthorized();
            }
            var response = await _userProfileService.UpdateProfile(profile, Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            string hostUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/";
            if (response.Data is not null)
            {
                response.Data.ProfilePicture = $"{hostUrl}{response.Data.ProfilePicture}";
                response.Data.BackPicture = $"{hostUrl}{response.Data.BackPicture}";
                return Ok(response);

            }
            return BadRequest(response);
        }
    }
}
