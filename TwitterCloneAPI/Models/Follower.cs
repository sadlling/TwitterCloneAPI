using System;
using System.Collections.Generic;

namespace TwitterCloneAPI.Models;

public partial class Follower
{
    public int FollowerId { get; set; }

    public int UserId { get; set; }

    public int FollowerUserId { get; set; }

    public byte[]? CreatedAt { get; set; }

    public virtual UserAuthentification FollowerUser { get; set; } = null!;

    public virtual UserAuthentification User { get; set; } = null!;
}
