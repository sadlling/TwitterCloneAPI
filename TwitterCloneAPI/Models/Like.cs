using System;
using System.Collections.Generic;

namespace TwitterCloneAPI.Models;

public partial class Like
{
    public int LikeId { get; set; }

    public int UserId { get; set; }

    public int TweetId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Tweet Tweet { get; set; } = null!;

    public virtual UserAuthentication User { get; set; } = null!;
}
