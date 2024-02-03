using TwitterCloneAPI.Models.NotificationResponse;
using TwitterCloneAPI.Models.ServiceResponse;

namespace TwitterCloneAPI.Services.Notifications
{
    public interface INotificationService
    {
        public Task<bool> AddTweetNotification(int userId,int tweetId,string type);
        public Task<bool> AddFollowNotification(int userId,int sourseUserId,string type);
        public Task<ResponseModel<List<NotificationResponseModel>>> UpdateNotifications(int[] notificationIds);
        public Task<ResponseModel<List<NotificationResponseModel>>> GetAllNotifications(int userId);
        public Task<ResponseModel<List<NotificationResponseModel>>> GetUnreadNotifications(int userId);
    }
}
