using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TwitterCloneAPI.Services.CommentLikes;

namespace TwitterCloneAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CommentLikeController : ControllerBase
    {
        private readonly ICommentLikeService _commentLikeService;
        public CommentLikeController(ICommentLikeService commentLikeService)
        {
            _commentLikeService = commentLikeService;
        }

        [HttpPost("AddLikeOnComment{commentId}")]
        public async Task<IActionResult> AddLikeOnComment(int commentId)
        {
            if (Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)) <= 0)
            {
                return Unauthorized();
            }
            var responce = await _commentLikeService.AddLikeOnComment(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)), commentId);
            if (responce.Success)
            {
                return Ok(responce);
            }
            return BadRequest(responce);
        }

        [HttpDelete("DeleteLikeFromComment{commentId}")]
        public async Task<IActionResult> DeleteLikeFromComment(int commentId)
        {
            if (Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)) <= 0)
            {
                return Unauthorized();
            }
            var responce = await _commentLikeService.DeleteLikeFromComment(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)), commentId);
            if (responce.Success)
            {
                return Ok(responce);
            }
            return BadRequest(responce);
        }
    }
}
