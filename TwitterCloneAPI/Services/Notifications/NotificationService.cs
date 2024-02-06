using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using TwitterCloneAPI.Models;
using TwitterCloneAPI.Models.NotificationResponse;
using TwitterCloneAPI.Models.ServiceResponse;

namespace TwitterCloneAPI.Services.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly TwitterCloneContext _context;
        public NotificationService(TwitterCloneContext context)
        {
            _context = context;
        }

        public async Task<bool> AddFollowNotification(int userId, int sourseUserId, string notificationType)
        {
            try
            {
                var notificationTypeId = await _context.NotificationTypes.Where(x => x.Name!.ToLower() == notificationType).Select(x => x.TypeId).FirstAsync();
                await _context.AddAsync(new Notification
                {
                    UserId = sourseUserId,
                    SourseUserId = userId,
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

        public async Task<bool> AddTweetNotification(int userId, int tweetId, string notificationType)
        {

            try
            {
                var sourseUserId = await _context.Tweets.Where(x => x.TweetId == tweetId).Select(x => x.UserId).FirstAsync();
                var notificationTypeId = await _context.NotificationTypes.Where(x => x.Name!.ToLower() == notificationType).Select(x => x.TypeId).FirstAsync();
                await _context.AddAsync(new Notification
                {
                    UserId = sourseUserId,
                    SourseUserId = userId,
                    TweetId = tweetId,
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

        public async Task<ResponseModel<List<NotificationResponseModel>>> GetAllNotifications(int userId)
        {
            var response = new ResponseModel<List<NotificationResponseModel>>();
            try
            {
                response.Data = await _context.Notifications.
                    Include(x => x.User).
                    Where(x => x.UserId == userId).
                    Select(x => new NotificationResponseModel
                    {
                        NotificationId = x.NotificationId,
                        UserId = userId,
                        TweetId = x.TweetId ?? 0,
                        SourseUserId = x.SourseUserId,
                        SourseUserName = !string.IsNullOrEmpty(x.SourseUser.UserProfile!.FullName) ? x.SourseUser.UserProfile!.FullName : x.SourseUser.UserProfile!.UserName ?? "",
                        SourseUserImage = x.SourseUser.UserProfile!.ProfilePicture!.Replace("\\", "/").Replace("wwwroot/", "") ?? "",
                        NotificationType = GetNotificationText(x.NotificationTypeNavigation.Name ?? ""),
                        CreatedAt = x.CreatedAt,
                        IsRead = x.IsReading
                    }).OrderByDescending(x => x.CreatedAt).ToListAsync();
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
                        NotificationType = GetNotificationText(x.NotificationTypeNavigation.Name ?? ""),
                        CreatedAt = x.CreatedAt,
                        IsRead = x.IsReading
                    }).OrderByDescending(x => x.CreatedAt).ToListAsync();
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

        public async Task<ResponseModel<List<NotificationResponseModel>>> UpdateNotifications(int[] notificationIds)
        {
            var response = new ResponseModel<List<NotificationResponseModel>>();
            try
            {
                var notificationToUpdate = await _context.Notifications.
                    Include(u => u.SourseUser.UserProfile).
                    Include(n => n.NotificationTypeNavigation).
                    Where(x => notificationIds.Contains(x.NotificationId)).
                    ToListAsync();

                notificationToUpdate.ForEach(notification => notification.IsReading = true);
                await _context.SaveChangesAsync();

                response.Data = notificationToUpdate.Select(x => new NotificationResponseModel
                {
                    NotificationId = x.NotificationId,
                    UserId = x.UserId,
                    TweetId = x.TweetId ?? 0,
                    SourseUserId = x.SourseUserId,
                    SourseUserName = !string.IsNullOrEmpty(x.SourseUser.UserProfile!.FullName) ? x.SourseUser.UserProfile!.FullName : x.SourseUser.UserProfile!.UserName ?? "",
                    SourseUserImage = x.SourseUser.UserProfile!.ProfilePicture!.Replace("\\", "/").Replace("wwwroot/", "") ?? "",
                    NotificationType = GetNotificationText(x.NotificationTypeNavigation?.Name ?? ""),
                    CreatedAt = x.CreatedAt,
                    IsRead = x.IsReading
                }).OrderByDescending(x => x.CreatedAt).ToList();

                response.Success = true;
                response.Message = "Updated notifications";

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        private static string GetNotificationText(string notificationType) => notificationType switch
        {
            "Like" => "Liked your tweet",
            "Retweet" => "Retweeted your tweet",
            "Comment" => "Commented your tweet",
            "Follow" => "Subscribed to you",
            _ => notificationType
        };

    }
}
