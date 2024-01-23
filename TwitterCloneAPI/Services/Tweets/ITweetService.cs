using TwitterCloneAPI.Models;
using TwitterCloneAPI.Models.ServiceResponse;
using TwitterCloneAPI.Models.TweetRequest;
using TwitterCloneAPI.Models.TweetResponse;

namespace TwitterCloneAPI.Services.Tweets
{
    public interface ITweetService
    {
        public Task<ResponseModel<TweetResponseModel>> CreateTweet(TweetRequestModel request,int userId);
        public Task<ResponseModel<List<TweetResponseModel>>> GetAllTweets(int userId);
        public Task<ResponseModel<TweetResponseModel>> UpdateTweet(UpdateTweetRequestModel request,int userId,int tweetId);
        public Task<ResponseModel<List<TweetResponseModel>>> GetCurrentUserTweetsAndRetweets(int userId,int currentUserId);
        public Task<ResponseModel<int>> DeleteTweet(int tweetId);
    }
}
