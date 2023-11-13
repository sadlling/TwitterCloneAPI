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
        public async Task<IActionResult> CreateTweet([FromForm]TweetRequestModel request)
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


    }
}
