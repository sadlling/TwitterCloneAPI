using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwitterCloneAPI.Services.HashtagsService;

namespace TwitterCloneAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HashtagController : ControllerBase
    {
        private readonly IHashtagService _hashtagService;
        public HashtagController(IHashtagService hashtagService)
        {
            _hashtagService = hashtagService;
        }

        [HttpGet("GetTopHashtags")]
        public async Task<IActionResult> GetTopHastags() 
        {
            var response = await _hashtagService.GetHashtags();
            if(response.Data is not null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

    }
}
