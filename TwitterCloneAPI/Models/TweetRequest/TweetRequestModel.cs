namespace TwitterCloneAPI.Models.TweetRequest
{
    public class TweetRequestModel
    {
        public string Content {  get; set; } = string.Empty!;
        public IFormFile? TweetImage { get; set; } = null!;
        public bool IsPublic { get; set; } = true;
        public string[] Hashtags { get; set; } = null!;
    }
}
