namespace TwitterCloneAPI.Models.CommentResponse
{
    public class CommentResponseModel
    {
        public int CommentId { get; set; }
        public int PosterUserId { get; set; }
        public string PostedUserName { get; set; } = string.Empty!;
        public string PostedUserImage { get; set; } = string.Empty!;
        public string Content { get; set; } = string.Empty!;
        public string Image { get; set; } = string.Empty!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int LikesCount { get; set; }
        public bool IsOwner { get; set; }
        public bool IsLiked { get; set; }
    }
}
