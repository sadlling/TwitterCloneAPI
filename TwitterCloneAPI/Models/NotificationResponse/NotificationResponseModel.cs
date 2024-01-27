namespace TwitterCloneAPI.Models.NotificationResponse
{
    public class NotificationResponseModel
    {
        public int NotificationId { get; set; }
        public int UserId {  get; set; }
        public int TweetId {  get; set; }
        public int SourseUserId {  get; set; }
        public string SourseUserName { get; set; } = null!;
        public string SourseUserImage {  get; set; } = null!;
        public string NotificationType { get; set; } = null!;
        public DateTime? CreatedAt { get; set; } = null!;
        public bool  IsRead { get; set; } = false;
    }
}
