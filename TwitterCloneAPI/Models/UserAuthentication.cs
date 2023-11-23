using System;
using System.Collections.Generic;

namespace TwitterCloneAPI.Models;

public partial class UserAuthentication
{
    public int UserId { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string RefreshToken { get; set; } = null!;

    public DateTime TokenCreated { get; set; }

    public DateTime TokenExpires { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Follower> FollowerFollowerUsers { get; set; } = new List<Follower>();//followers

    public virtual ICollection<Follower> FollowerUsers { get; set; } = new List<Follower>();//following

    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();

    public virtual ICollection<Notification> NotificationSourseUsers { get; set; } = new List<Notification>();

    public virtual ICollection<Notification> NotificationUsers { get; set; } = new List<Notification>();

    public virtual ICollection<Retweet> Retweets { get; set; } = new List<Retweet>();

    public virtual ICollection<SavedTweet> SavedTweets { get; set; } = new List<SavedTweet>();

    public virtual ICollection<Tweet> Tweets { get; set; } = new List<Tweet>();

    public virtual UserProfile? UserProfile { get; set; }
}
