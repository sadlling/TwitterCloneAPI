using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TwitterCloneAPI.Services.Notifications;

namespace TwitterCloneAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        [HttpGet("GetAllNotifications")]
        public async Task<IActionResult> GetAllNotifications()
        {
            if (Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)) <= 0)
            {
                return Unauthorized();
            }
            var responce = await _notificationService.GetAllNotifications(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            string hostUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/";
            if (responce.Data is not null)
            {
                responce.Data.ForEach(x =>
                {
                    if (!string.IsNullOrEmpty(x.SourseUserImage))
                    {
                        x.SourseUserImage = $"{hostUrl}{x.SourseUserImage}";
                    }
                });
                return Ok(responce);
            }
            if (responce.Data is null && responce.Success)
            {
                return Ok(responce);
            }
            return BadRequest(responce);
        }

        [HttpGet("GetUnreadNotifications")]
        public async Task<IActionResult> GetUnreadNotifications()
        {
            if (Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)) <= 0)
            {
                return Unauthorized();
            }
            var responce = await _notificationService.GetUnreadNotifications(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            string hostUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/";
            if (responce.Data is not null)
            {
                responce.Data.ForEach(x =>
                {
                    if (!string.IsNullOrEmpty(x.SourseUserImage))
                    {
                        x.SourseUserImage = $"{hostUrl}{x.SourseUserImage}";
                    }
                });
                return Ok(responce);
            }
            if (responce.Data is null && responce.Success)
            {
                return Ok(responce);
            }
            return BadRequest(responce);
        }
    }
}
