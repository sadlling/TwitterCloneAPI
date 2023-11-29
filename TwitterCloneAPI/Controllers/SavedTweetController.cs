using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TwitterCloneAPI.Services.SavedTweets;

namespace TwitterCloneAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SavedTweetController : ControllerBase
    {
        private readonly ISavedTweetSevice savedTweetSevice;
        public SavedTweetController(ISavedTweetSevice savedTweetSevice)
        {
            this.savedTweetSevice = savedTweetSevice;
        }

        [HttpPost("AddTweetInSaved{tweetId}")]
        public async Task<IActionResult> AddTweetInSaved(int tweetId)
        {
            if (Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)) <= 0)
            {
                return Unauthorized();
            }
            var responce = await savedTweetSevice.AddTweetInSaved(tweetId, Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            if (responce.Success)
            {
                return Ok(responce);
            }
            return BadRequest(responce);
        }

        [HttpDelete("DeleteTweetFromSaved{tweetId}")]
        public async Task<IActionResult> DeleteTweetFromSaved(int tweetId)
        {
            if (Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)) <= 0)
            {
                return Unauthorized();
            }
            var responce = await savedTweetSevice.DeleteTweetInSaved(tweetId, Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            if (responce.Success)
            {
                return Ok(responce);
            }
            return BadRequest(responce);
        }
    }
}
