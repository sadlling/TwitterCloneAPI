using TwitterCloneAPI.Models.ServiceResponse;

namespace TwitterCloneAPI.Services.Likes
{
    public interface ILikeService
    {
        public Task<ResponseModel<int>> AddTweetInLiked(int userId, int tweetId);
        public Task<ResponseModel<int>> DeleteTweetFromLiked(int userId, int tweetId);
    }
}
