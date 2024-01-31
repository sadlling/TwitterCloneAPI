using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TwitterCloneAPI.Services.Likes;
using TwitterCloneAPI.Services.Notifications;

namespace TwitterCloneAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly ILikeService _likeService;
        private readonly INotificationService _notificationService;
        public LikeController(ILikeService likeService, INotificationService notificationService)
        {
            _likeService = likeService;
            _notificationService = notificationService;
        }
        [HttpPost("AddTweetInLiked{tweetId}")]
        public async Task<IActionResult> AddTweetInLiked(int tweetId)
        {
            if (Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)) <= 0)
            {
                return Unauthorized();
            }
            var responce = await _likeService.AddTweetInLiked(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)), tweetId);
            if (responce.Success)
            {
                if(await _notificationService.AddTweetNotification(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)),tweetId,"Like"))
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

        [HttpDelete("DeleteTweetFromLiked{tweetId}")]
        public async Task<IActionResult> DeleteTweetFromLiked(int tweetId)
        {
            if (Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)) <= 0)
            {
                return Unauthorized();
            }
            var responce = await _likeService.DeleteTweetFromLiked(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)), tweetId);
            if (responce.Success)
            {
                return Ok(responce);
            }
            return BadRequest(responce);
        }

    }
}
