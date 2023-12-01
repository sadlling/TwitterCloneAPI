using TwitterCloneAPI.Models.ServiceResponse;
using TwitterCloneAPI.Models.TweetResponse;

namespace TwitterCloneAPI.Services.Retweets
{
    public interface IRetweetService
    {
        public Task<ResponseModel<int>> AddTweetInRetweets(int userId, int tweetId);
        public Task<ResponseModel<int>> RemoveTweetFromRetweets(int userId, int tweetId);
    }
}
