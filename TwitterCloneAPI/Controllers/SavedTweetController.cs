using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using TwitterCloneAPI.Models.TweetResponse;
using TwitterCloneAPI.Paging;
using TwitterCloneAPI.Services.SavedTweets;

namespace TwitterCloneAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SavedTweetController : ControllerBase
    {
        private readonly ISavedTweetSevice _savedTweetSevice;
        public SavedTweetController(ISavedTweetSevice savedTweetSevice)
        {
            _savedTweetSevice = savedTweetSevice;
        }

        [HttpPost("AddTweetInSaved{tweetId}")]
        public async Task<IActionResult> AddTweetInSaved(int tweetId)
        {
            if (Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)) <= 0)
            {
                return Unauthorized();
            }
            var responce = await _savedTweetSevice.AddTweetInSaved(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)), tweetId);
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
            var responce = await _savedTweetSevice.DeleteTweetInSaved(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)), tweetId);
            if (responce.Success)
            {
                return Ok(responce);
            }
            return BadRequest(responce);
        }
        [HttpGet("GetSavedTweets")]
        public async Task<IActionResult> GetSavedTweets([FromQuery]QueryStringParameters parameters)
        {
            //TODO what return?? 200 or 400 if tweets null
            if (Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)) <= 0)
            {
                return Unauthorized();
            }
            var responce = await _savedTweetSevice.GetSavedTweets(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));
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
                var pagedResult = PagedList<TweetResponseModel>.ToPagedList(responce.Data, parameters.PageNumber, parameters.PageSize);
                var metadata = new
                {
                    pagedResult.TotalCount,
                    pagedResult.PageSize,
                    pagedResult.CurrentPage,
                    pagedResult.TotalPages,
                    pagedResult.HasNext,
                    pagedResult.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(pagedResult);
            }
            if(responce.Data is null && responce.Success)
            {
                return Ok(responce);
            }
            return BadRequest(responce);
        }

    }
}
