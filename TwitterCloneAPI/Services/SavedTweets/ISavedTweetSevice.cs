using TwitterCloneAPI.Models.ServiceResponse;
using TwitterCloneAPI.Models.TweetResponse;

namespace TwitterCloneAPI.Services.SavedTweets
{
    public interface ISavedTweetSevice
    {
        public Task<ResponseModel<int>> AddTweetInSaved(int tweetId, int userId);
        public Task<ResponseModel<int>> DeleteTweetInSaved(int tweetId,int userId);
        public Task<ResponseModel<List<TweetResponseModel>>> GetSavedTweets(int userId);

    }
}
