using TwitterCloneAPI.Models.NotificationResponse;
using TwitterCloneAPI.Models.ServiceResponse;

namespace TwitterCloneAPI.Services.Notifications
{
    public interface INotificationService
    {
        public Task<bool> AddNotification(int userId,int tweetId,string type);
        public Task<ResponseModel<List<NotificationResponseModel>>> GetAllNotifications(int userId);
    }
}
