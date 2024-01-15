using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TwitterCloneAPI.Models.CommentRequest;
using TwitterCloneAPI.Models.TweetRequest;
using TwitterCloneAPI.Services.Comments;

namespace TwitterCloneAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost("CreateComment{tweetId}")]
        public async Task<IActionResult> CreateTweet([FromForm] CommentRequestModel request,int tweetId)
        {
            if (Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)) <= 0)
            {
                return Unauthorized();
            }
            var response = await _commentService.CreateComment(request, Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)),tweetId);
            string hostUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/";
            if (response.Data is not null)
            {
                if (!string.IsNullOrEmpty(response.Data.Image))
                {
                    response.Data.Image = $"{hostUrl}{response.Data.Image}";
                }
                return Ok(response);
            }
            return BadRequest(response);

        }

        [HttpGet("GetTweetComments{tweetId}")]
        public async Task<IActionResult> GetTweetComments(int tweetId)
        {
            var responce = await _commentService.GetTweetComments(tweetId);
            string hostUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/";
            if (responce.Data is not null)
            {
                responce.Data.ForEach(x =>
                {
                    if (!string.IsNullOrEmpty(x.Image))
                        x.Image = $"{hostUrl}{x.Image}";
                });
                return Ok(responce);
            }
            return BadRequest(responce);

        }
    }
}
