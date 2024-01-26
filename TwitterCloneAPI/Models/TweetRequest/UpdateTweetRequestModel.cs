namespace TwitterCloneAPI.Models.TweetRequest
{
    public class UpdateTweetRequestModel
    {
        public string Content { get; set; } = string.Empty!;
        public IFormFile? NewTweetImage { get; set; } = null!;
        public string? OldTweetImage { get; set; } = null!;
        public string[]? Hashtags { get; set; } = null!;
        public bool IsPublic { get; set; } = true;
    }
}
