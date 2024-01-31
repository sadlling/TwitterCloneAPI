using Microsoft.EntityFrameworkCore;
using TwitterCloneAPI.Models;
using TwitterCloneAPI.Models.NotificationResponse;
using TwitterCloneAPI.Models.ServiceResponse;

namespace TwitterCloneAPI.Services.Notifications
{
    public class NotificationService:INotificationService
    {
        private readonly TwitterCloneContext _context;
        public NotificationService(TwitterCloneContext context)
        {
            _context = context;
        }

        public async Task<bool> AddFollowNotification(int userId,int sourseUserId, string notificationType)
        {
            try
            {
                var notificationTypeId = await _context.NotificationTypes.Where(x => x.Name!.ToLower() == notificationType).Select(x => x.TypeId).FirstAsync();
                await _context.AddAsync(new Notification
                {
                    UserId = userId,
                    SourseUserId = sourseUserId,
                    NotificationType = notificationTypeId,
                    IsReading = false,
                    CreatedAt = DateTime.Now,
                });
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> AddTweetNotification(int userId,int tweetId,string notificationType)
        {
            try
            {
                var sourseUserId = await _context.Tweets.Where(x=>x.TweetId==tweetId).Select(x=>x.UserId).FirstAsync();
                var notificationTypeId = await _context.NotificationTypes.Where(x => x.Name!.ToLower() == notificationType).Select(x => x.TypeId).FirstAsync();
                await _context.AddAsync( new Notification
                {
                    UserId = userId,
                    SourseUserId = sourseUserId,
                    TweetId = tweetId,
                    NotificationType =  notificationTypeId,
                    IsReading = false,
                    CreatedAt = DateTime.Now,
                });
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;

        }

        public  async Task<ResponseModel<List<NotificationResponseModel>>> GetAllNotifications(int userId)
        {
            var response = new ResponseModel<List<NotificationResponseModel>>();
            try
            {
                response.Data = await _context.Notifications.
                    Include(x=>x.User).
                    Where(x=>x.UserId == userId).
                    Select(x=> new NotificationResponseModel 
                    {
                      NotificationId = x.NotificationId,
                      UserId = userId,
                      TweetId = x.TweetId ?? 0,
                      SourseUserId=x.SourseUserId,
                      SourseUserName = !string.IsNullOrEmpty(x.SourseUser.UserProfile!.FullName) ? x.SourseUser.UserProfile!.FullName : x.SourseUser.UserProfile!.UserName ?? "",
                      SourseUserImage = x.SourseUser.UserProfile!.ProfilePicture!.Replace("\\", "/").Replace("wwwroot/", "") ?? "",
                      NotificationType = x.NotificationTypeNavigation.Name?? "",
                      CreatedAt = x.CreatedAt,
                      IsRead = x.IsReading
                    }).ToListAsync();
                response.Success = true;
                response.Message = "All notifications";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
               
            }
            return response;
        }
        public async Task<ResponseModel<List<NotificationResponseModel>>> GetUnreadNotifications(int userId)
        {
            var response = new ResponseModel<List<NotificationResponseModel>>();
            try
            {
                response.Data = await _context.Notifications.
                    Include(x => x.User).
                    Where(x => x.UserId == userId && !x.IsReading).
                    Select(x => new NotificationResponseModel
                    {
                        NotificationId = x.NotificationId,
                        UserId = userId,
                        TweetId = x.TweetId ?? 0,
                        SourseUserId = x.SourseUserId,
                        SourseUserName = !string.IsNullOrEmpty(x.SourseUser.UserProfile!.FullName) ? x.SourseUser.UserProfile!.FullName : x.SourseUser.UserProfile!.UserName ?? "",
                        SourseUserImage = x.SourseUser.UserProfile!.ProfilePicture!.Replace("\\", "/").Replace("wwwroot/", "") ?? "",
                        NotificationType = x.NotificationTypeNavigation.Name ?? "",
                        CreatedAt = x.CreatedAt,
                        IsRead = x.IsReading
                    }).ToListAsync();
                response.Success = true;
                response.Message = "Unread notifications";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;

            }
            return response;
        }
    }
}
