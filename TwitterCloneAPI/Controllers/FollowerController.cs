﻿using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TwitterCloneAPI.Models;
using TwitterCloneAPI.Services.Folowers;
using TwitterCloneAPI.Services.Notifications;

namespace TwitterCloneAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FollowerController : ControllerBase
    {
        private readonly IFolowerService _followerService;
        private readonly INotificationService _notificationService;
        public FollowerController(IFolowerService followerService, INotificationService notificationService) 
        {
            _notificationService = notificationService;
            _followerService = followerService;
        }

        [HttpPost("Subscribe{followerId}")]
        public async Task<IActionResult> AddFollow(int followerId)
        {
            if (Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)) <= 0)
            {
                return Unauthorized();
            }
            var responce = await _followerService.AddFollower(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)), followerId);
            if (responce.Success)
            {
                if (await _notificationService.AddFollowNotification(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)), followerId, "Follow"))
                {
                    responce.Message += " And added notification!";
                }
                else
                {
                    responce.Message += " Notification not added(";
                }
                return Ok(responce);
            }
            return BadRequest(responce);
        }
        [HttpDelete("Unsubscribe{followerId}")]

        public async Task<IActionResult> RemoveFollow(int followerId)
        {
            if (Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)) <= 0)
            {
                return Unauthorized();
            }
            var responce = await _followerService.RemoveFollower(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)), followerId);
            if (responce.Success)
            {
                return Ok(responce);
            }
            return BadRequest(responce);
        }

        [HttpGet("GetFollowersTweets")]
        public async Task<IActionResult> GetFollowersTweets()
        {
            if (Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)) <= 0)
            {
                return Unauthorized();
            }
            var responce = await _followerService.GetFollowersTweets(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            string hostUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/";
            if (responce.Data is not null)
            {
                responce.Data.ForEach(x =>
                {
                    if (!string.IsNullOrEmpty(x.Image))
                    {
                        x.Image = $"{hostUrl}{x.Image}";
                    }
                    if (!string.IsNullOrEmpty(x.PostedUserImage))
                    {
                        x.PostedUserImage = $"{hostUrl}{x.PostedUserImage}";
                    }
                });
                return Ok(responce);
            }
            return BadRequest(responce);
        }
        [HttpGet("GetFollowersTweetsByParams")]
        public async Task<IActionResult> GetFollowersTweetsByParams([FromQuery(Name = "page")] string parameter)
        {
            if (Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)) <= 0)
            {
                return Unauthorized();
            }
            var responce = await _followerService.GetFollowersTweets(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            string hostUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/";
            if (responce.Data is not null)
            {
                responce.Data.ForEach(x =>
                {
                    if (!string.IsNullOrEmpty(x.Image))
                    {
                        x.Image = $"{hostUrl}{x.Image}";
                    }
                    if (!string.IsNullOrEmpty(x.PostedUserImage))
                    {
                        x.PostedUserImage = $"{hostUrl}{x.PostedUserImage}";
                    }
                });
                if (parameter.ToLower() == "latest")
                {
                    return Ok(responce);
                }
                if (parameter.ToLower() == "top")
                {
                    responce.Data = responce.Data.OrderByDescending(x => x.LikesCount).ToList();
                    return Ok(responce);
                }
                if (parameter.ToLower() == "media")
                {
                    responce.Data = responce.Data.Where(x => !string.IsNullOrEmpty(x.Image)).ToList();
                    return Ok(responce);
                }
            }
            return BadRequest(responce);
        }
    }
}
