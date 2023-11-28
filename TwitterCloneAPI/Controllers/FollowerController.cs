using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TwitterCloneAPI.Services.Folowers;

namespace TwitterCloneAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FollowerController : ControllerBase
    {
        private readonly IFolowerService _followerService;
        public FollowerController(IFolowerService followerService)
        {
            _followerService = followerService;
        }

        [HttpPost("Subscribe{followerId}")]
        public async Task<IActionResult> AddFollow(int followerId)
        {
            if (Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)) <= 0)
            {
                return Unauthorized();
            }
            var responce = await _followerService.AddFollower(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)), followerId);
            if (responce.Success)
            {
                return Ok(responce);
            }
            return BadRequest(responce);
        }
        [HttpDelete("Unsubscribe{followerId}")]

        public async Task<IActionResult> RemoveFollow(int followerId)
        {
            if (Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)) <= 0)
            {
                return Unauthorized();
            }
            var responce = await _followerService.RemoveFollower(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)), followerId);
            if (responce.Success)
            {
                return Ok(responce);
            }
            return BadRequest(responce);
        }
    }
}
