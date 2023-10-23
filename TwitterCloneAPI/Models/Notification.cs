using System;
using System.Collections.Generic;

namespace TwitterCloneAPI.Models;

public partial class Notification
{
    public int NotificationId { get; set; }

    public int UserId { get; set; }

    public int NotificationType { get; set; }

    public int SourseUserId { get; set; }

    public int TweetId { get; set; }

    public bool IsReading { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual NotificationType NotificationTypeNavigation { get; set; } = null!;

    public virtual UserAuthentification SourseUser { get; set; } = null!;

    public virtual Tweet Tweet { get; set; } = null!;

    public virtual UserAuthentification User { get; set; } = null!;
}
