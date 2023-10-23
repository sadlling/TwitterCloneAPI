using System;
using System.Collections.Generic;

namespace TwitterCloneAPI.Models;

public partial class UserProfile
{
    public int ProfileId { get; set; }

    public string? UserName { get; set; }

    public string? FullName { get; set; }

    public string? Bio { get; set; }

    public string? ProfilePicture { get; set; }

    public string? BackPicture { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual UserAuthentification Profile { get; set; } = null!;
}
