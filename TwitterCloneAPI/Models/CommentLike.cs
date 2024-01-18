namespace TwitterCloneAPI.Models
{
    public class CommentLike
    {
        public int CommentLikeId { get; set; }
        public int UserId { get; set; }
        public int CommentId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public virtual Comment Comment { get; set; } = null!;
        public virtual UserAuthentication User { get; set; } = null!;
    }
}
