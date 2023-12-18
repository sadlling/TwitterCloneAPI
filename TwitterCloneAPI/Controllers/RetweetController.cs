using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TwitterCloneAPI.Services.Retweets;

namespace TwitterCloneAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RetweetController : ControllerBase
    {
        private readonly IRetweetService _retweetsService;
        public RetweetController(IRetweetService retweetsService)
        {
            _retweetsService = retweetsService;
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
