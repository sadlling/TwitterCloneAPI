using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TwitterCloneAPI.Models;
using TwitterCloneAPI.Models.TweetRequest;
using TwitterCloneAPI.Services.Tweets;

namespace TwitterCloneAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TweetController : ControllerBase
    {
        private readonly ITweetService _tweetService;
        public TweetController(ITweetService tweetService)
        {
            _tweetService = tweetService;
        }

        [HttpPut("CreateTweet")]
        public async Task<IActionResult> CreateTweet([FromForm] TweetRequestModel request)
        {
            if (Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)) <= 0)
            {
                return Unauthorized();
            }
            var tweet = await _tweetService.CreateTweet(request, Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));

            if (tweet.Data is not null)
            {
                return Ok(tweet);
            }
            return BadRequest(request);

        }

        [HttpPost("AddTweetInSaved{tweetId}")]
        public async Task<IActionResult> AddTweetInSaved(int tweetId)
        {
            if (Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)) <= 0)
            {
                return Unauthorized();
            }
            var responce = await _tweetService.AddTweetInSaved(tweetId, Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            if (responce.Data > 0)
            {
                return Ok(responce);
            }
            return BadRequest(responce);
        }

        [HttpGet("GetAllTweets")]
        public async Task<IActionResult> GetAllTweets()
        {
            var responce = await _tweetService.GetAllTweets();
            if(responce.Data is not null)
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
            var responce = await _tweetService.GetFollowersTweets(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier))); 
            if (responce.Data is not null)
            {
                return Ok(responce);
            }
            return BadRequest(responce);
        }




    }
}
