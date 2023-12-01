using TwitterCloneAPI.Models.ServiceResponse;
using TwitterCloneAPI.Models.TweetResponse;

namespace TwitterCloneAPI.Services.SavedTweets
{
    public interface ISavedTweetSevice
    {
        public Task<ResponseModel<int>> AddTweetInSaved(int userId, int tweetId);
        public Task<ResponseModel<int>> DeleteTweetInSaved(int userId, int tweetId);
        public Task<ResponseModel<List<TweetResponseModel>>> GetSavedTweets(int userId);

    }
}
