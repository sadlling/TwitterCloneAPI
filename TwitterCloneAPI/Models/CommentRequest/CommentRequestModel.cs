namespace TwitterCloneAPI.Models.CommentRequest
{
    public class CommentRequestModel
    {
        public int TweetId { get; set; }
        public string Content { get; set; } = string.Empty!;
        public string Image { get; set; } = string.Empty!;

    }
}
