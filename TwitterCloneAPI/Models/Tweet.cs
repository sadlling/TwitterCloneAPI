using System;
using System.Collections.Generic;

namespace TwitterCloneAPI.Models;

public partial class Tweet
{
    public int TweetId { get; set; }

    public int UserId { get; set; }

    public string Content { get; set; } = null!;

    public string? TweetImage { get; set; }

    public bool IsPublic { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Retweet> Retweets { get; set; } = new List<Retweet>();

    public virtual ICollection<SavedTweet> SavedTweets { get; set; } = new List<SavedTweet>();

    public virtual ICollection<TweetHashtag> TweetHashtags { get; set; } = new List<TweetHashtag>();

    public virtual UserAuthentication User { get; set; } = null!;
}
