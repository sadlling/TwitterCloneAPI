using System;
using System.Collections.Generic;

namespace TwitterCloneAPI.Models;

public partial class Comment
{
    public int CommentId { get; set; }

    public int UserId { get; set; }

    public int TweetId { get; set; }

    public string? Content { get; set; }

    public string? CommentImage { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public virtual Tweet Tweet { get; set; } = null!;

    public virtual UserAuthentication User { get; set; } = null!;
}
