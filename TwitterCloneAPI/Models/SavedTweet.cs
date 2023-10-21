using System;
using System.Collections.Generic;

namespace TwitterCloneAPI.Models;

public partial class SavedTweet
{
    public int SavedTweetId { get; set; }

    public int UserId { get; set; }

    public int TweetId { get; set; }

    public byte[]? CreatedAt { get; set; }

    public virtual Tweet Tweet { get; set; } = null!;

    public virtual UserAuthentification User { get; set; } = null!;
}
