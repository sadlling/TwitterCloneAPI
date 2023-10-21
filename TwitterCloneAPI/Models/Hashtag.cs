using System;
using System.Collections.Generic;

namespace TwitterCloneAPI.Models;

public partial class Hashtag
{
    public int HashtagId { get; set; }

    public string Name { get; set; } = null!;

    public byte[]? CreatedAt { get; set; }

    public virtual ICollection<TweetHashtag> TweetHashtags { get; set; } = new List<TweetHashtag>();
}
