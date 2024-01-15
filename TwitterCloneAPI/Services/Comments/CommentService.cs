using TwitterCloneAPI.Models;

namespace TwitterCloneAPI.Services.Comments
{
    public class CommentService:ICommentService
    {
        private readonly TwitterCloneContext _context;
        public CommentService(TwitterCloneContext context)
        {
            _context = context;
        }
    }
}
