using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TwitterCloneAPI.Services.Notifications;
using TwitterCloneAPI.Services.Retweets;

namespace TwitterCloneAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RetweetController : ControllerBase
    {
        private readonly IRetweetService _retweetsService;
        private readonly INotificationService _notificationService;
        public RetweetController(IRetweetService retweetsService, INotificationService notificationService)
        {
            _retweetsService = retweetsService;
            _notificationService = notificationService;
        }

        [HttpPost("AddTweetInRetweets{tweetId}")]
        public async Task<IActionResult> AddTweetInRetweets(int tweetId)
        {
            if (Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)) <= 0)
            {
                return Unauthorized();
            }
            var responce = await _retweetsService.AddTweetInRetweets(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)),tweetId);
            if (responce.Success)
            {
                if (await _notificationService.AddTweetNotification(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)), tweetId, "Retweet"))
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

        [HttpDelete("DeleteTweetFromRetweet{tweetId}")]
        public async Task<IActionResult> DeleteTweetFromRetweets(int tweetId)
        {
            if (Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)) <= 0)
            {
                return Unauthorized();
            }
            var responce = await _retweetsService.RemoveTweetFromRetweets(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)), tweetId);
            if (responce.Success)
            {
                return Ok(responce);
            }
            return BadRequest(responce);
        }
    }
}
