using System;
using System.Collections.Generic;

namespace TwitterCloneAPI.Models;

public partial class TweetHashtag
{
    public int Id { get; set; }

    public int HashtagId { get; set; }

    public int TweetId { get; set; }

    public virtual Hashtag Hashtag { get; set; } = null!;

    public virtual Tweet Tweet { get; set; } = null!;
}
