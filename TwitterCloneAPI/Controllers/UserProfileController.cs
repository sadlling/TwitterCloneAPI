﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
            var userProfile = await _userProfileService.GetProfileByUserId(userId);
            if(userProfile.Data is not null)
            {
                return Ok(userProfile);
            }
            return NotFound(userProfile);
        }

        [HttpGet("GetCurrentUserProfile")]
        public async Task<IActionResult> GetCurrentUserProfile()
        {
            if (Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)) <= 0)
            {
                return Unauthorized();
            }
            var userProfile = await _userProfileService.GetProfileByUserId(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            if (userProfile.Data is not null)
            {
                return Ok(userProfile);
            }
            return NotFound(userProfile);
        }

        [HttpPut("UpdateUserProfile")]
        public async Task<IActionResult> UpdateUserProfile([FromForm]UpdateUserProfileRequest profile)
        {
            if (Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)) <= 0)
            {
                return Unauthorized();
            }
            var response = await _userProfileService.UpdateProfile(profile, Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            if (response.Data is not null)
            {
                return Ok(response);

            }
            return BadRequest(response);
        }
    }
}
