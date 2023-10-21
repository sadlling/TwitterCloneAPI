using System;
using System.Collections.Generic;

namespace TwitterCloneAPI.Models;

public partial class NotificationType
{
    public int TypeId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
