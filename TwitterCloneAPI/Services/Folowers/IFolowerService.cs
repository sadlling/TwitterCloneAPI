using TwitterCloneAPI.Models.ServiceResponse;
using TwitterCloneAPI.Models.TweetResponse;

namespace TwitterCloneAPI.Services.Folowers
{
    public interface IFolowerService
    {
        public Task<ResponseModel<int>> AddFollower(int userId,int followerId);
        public Task<ResponseModel<int>> RemoveFollower(int userId, int followerId);
        public Task<ResponseModel<List<TweetResponseModel>>> GetFollowersTweets(int userId);

    }
}
