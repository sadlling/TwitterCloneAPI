using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace TwitterCloneAPI.Models;

public partial class Hashtag
{
    public int HashtagId { get; set; }
    public string Name { get; set; } = null!;
    public DateTime? CreatedAt { get; set; }
    public virtual ICollection<TweetHashtag> TweetHashtags { get; set; } = new List<TweetHashtag>();
}
