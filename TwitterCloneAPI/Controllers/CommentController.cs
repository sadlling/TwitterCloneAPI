using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TwitterCloneAPI.Models.CommentRequest;
using TwitterCloneAPI.Models.TweetRequest;
using TwitterCloneAPI.Services.Comments;
using TwitterCloneAPI.Services.Notifications;

namespace TwitterCloneAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly INotificationService _notificationService;
        public CommentController(ICommentService commentService, INotificationService notificationService)
        {
            _commentService = commentService;
            _notificationService = notificationService;
        }

        [HttpPost("CreateComment{tweetId}")]
        public async Task<IActionResult> CreateComment([FromForm] CommentRequestModel request,int tweetId)
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
                if (!string.IsNullOrEmpty(response.Data.PostedUserImage))
                {
                    response.Data.PostedUserImage = $"{hostUrl}{response.Data.PostedUserImage}";
                }

                if (await _notificationService.AddTweetNotification(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)), tweetId, "Like"))
                {
                    response.Message += " And added notification!";
                }
                else
                {
                    response.Message += " Notification not added(";
                }

                return Ok(response);
            }
            return BadRequest(response);

        }

        [HttpGet("GetTweetComments{tweetId}")]
        public async Task<IActionResult> GetTweetComments(int tweetId)
        {
            var responce = await _commentService.GetTweetComments(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)),tweetId);
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

        [HttpDelete("DeleteComment{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            if (Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)) <= 0)
            {
                return Unauthorized();
            }
            var responce = await _commentService.DeleteComment(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)), commentId);
            if (responce.Success)
            {
                return Ok(responce);
            }
            return BadRequest(responce);
        }
    }
}
