using System;
using System.Collections.Generic;

namespace TwitterCloneAPI.Models;

public partial class Follower
{
    public int FollowerId { get; set; }

    public int UserId { get; set; }

    public int FollowerUserId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual UserAuthentication FollowerUser { get; set; } = null!;

    public virtual UserAuthentication User { get; set; } = null!;
}
