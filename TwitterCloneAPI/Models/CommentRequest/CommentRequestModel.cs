namespace TwitterCloneAPI.Models.CommentRequest
{
    public class CommentRequestModel
    {
        public string Content { get; set; } = string.Empty!;
        public IFormFile? Image { get; set; }

    }
}
