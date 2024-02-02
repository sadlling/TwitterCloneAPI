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
            var response = await _userProfileService.GetProfileByUserId(userId, Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            string hostUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/";
            if (response.Data is not null)
            {
                if (!string.IsNullOrEmpty(response.Data.ProfilePicture))
                {
                    response.Data.ProfilePicture = $"{hostUrl}{response.Data.ProfilePicture}";
                }
                if (!string.IsNullOrEmpty(response.Data.BackPicture))
                {
                    response.Data.BackPicture = $"{hostUrl}{response.Data.BackPicture}";
                }
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
                if (!string.IsNullOrEmpty(response.Data.ProfilePicture))
                {
                    response.Data.ProfilePicture = $"{hostUrl}{response.Data.ProfilePicture}";
                }
                if (!string.IsNullOrEmpty(response.Data.BackPicture))
                {
                    response.Data.BackPicture = $"{hostUrl}{response.Data.BackPicture}";
                }
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("GetTwoPopularProfiles")]
        public async Task<IActionResult> GetTwoPopularProfiles()
        {
            var response = await _userProfileService.GetTwoPopularProfiles(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            string hostUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/";
            if (response.Data is not null)
            {
                response.Data.ForEach(x =>
                {
                    if (!string.IsNullOrEmpty(x.ProfilePicture))
                    {
                        x.ProfilePicture = $"{hostUrl}{x.ProfilePicture}";
                    }
                    if (!string.IsNullOrEmpty(x.BackPicture))
                    {
                        x.BackPicture = $"{hostUrl}{x.BackPicture}";
                    }
                });
                return Ok(response);
            }
            return BadRequest(response);
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
                if (!string.IsNullOrEmpty(response.Data.ProfilePicture))
                {
                    response.Data.ProfilePicture = $"{hostUrl}{response.Data.ProfilePicture}";
                }
                if (!string.IsNullOrEmpty(response.Data.BackPicture))
                {
                    response.Data.BackPicture = $"{hostUrl}{response.Data.BackPicture}";
                }
                return Ok(response);

            }
            return BadRequest(response);
        }
    }
}
