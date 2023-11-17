using TwitterCloneAPI.Models;
using TwitterCloneAPI.Models.ServiceResponse;
using TwitterCloneAPI.Models.TweetRequest;
using TwitterCloneAPI.Models.TweetResponse;

namespace TwitterCloneAPI.Services.Tweets
{
    public interface ITweetService
    {
        public Task<ResponseModel<Tweet>> CreateTweet(TweetRequestModel request,int userId);
        public Task<ResponseModel<List<TweetResponseModel>>> GetAllTweets();
        public Task<ResponseModel<int>> AddTweetInSaved(int tweetId, int userId);
    }
}
