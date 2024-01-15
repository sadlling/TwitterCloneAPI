using TwitterCloneAPI.Models.CommentRequest;
using TwitterCloneAPI.Models.CommentResponse;
using TwitterCloneAPI.Models.ServiceResponse;

namespace TwitterCloneAPI.Services.Comments
{
    public interface ICommentService
    {
        public Task<ResponseModel<List<CommentResponseModel>>> GetTweetComments(int tweetId);
        public Task<ResponseModel<CommentResponseModel>> GetComment(int commentId);
        public Task<ResponseModel<CommentResponseModel>> CreateComment(CommentRequestModel request,int userId);
        public Task<ResponseModel<CommentResponseModel>> UpdateComment(CommentRequestModel request,int userId,int commentId);
    }
}
