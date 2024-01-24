﻿namespace TwitterCloneAPI.Models.NotificationResponse
{
    public class NotificationResponseModel
    {
        public int NotificationId { get; set; }
        public int SourseUserId {  get; set; }
        public string SourseUserName { get; set; } = null!;
        public string SourseUserPhoto {  get; set; } = null!;
        public string NotificationType { get; set; } = null!;
        public DateTime? CreatedAt { get; set; } = null!;
        public bool  IsRead { get; set; } = false;
    }
}
