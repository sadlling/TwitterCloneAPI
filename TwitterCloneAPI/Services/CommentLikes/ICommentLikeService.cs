using TwitterCloneAPI.Models.ServiceResponse;

namespace TwitterCloneAPI.Services.CommentLikes
{
    public interface ICommentLikeService
    {
        public Task<ResponseModel<int>> AddLikeInComment(int userId, int commentId);
        public Task<ResponseModel<int>> DeleteLikeFromComment(int userId, int commentId);

    }
}
