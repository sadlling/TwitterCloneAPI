using Microsoft.AspNetCore.Http.HttpResults;
using static System.Net.Mime.MediaTypeNames;
using TwitterCloneAPI.Models;

namespace TwitterCloneAPI.Models.TweetResponse
{
    public class TweetResponseModel
    {
        public int TweetId {  get; set; }
        public int PostedUserId { get; set; }
        public string Content { get; set; } = string.Empty!;
        public string Image { get; set; } = string.Empty!;
        public bool IsPublic { get; set; } = true;
        public DateTime CreatedAt {  get; set; }
        public int CommentsCount { get; set; }
        public int RetweetCount { get; set; }
        public int LikesCount { get; set; }

    }
}
