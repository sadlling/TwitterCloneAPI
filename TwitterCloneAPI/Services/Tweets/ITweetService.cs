using TwitterCloneAPI.Models;
using TwitterCloneAPI.Models.ServiceResponse;
using TwitterCloneAPI.Models.TweetRequest;

namespace TwitterCloneAPI.Services.Tweets
{
    public interface ITweetService
    {
        public Task<ResponseModel<Tweet>> CreateTweet(TweetRequestModel request,int userId); 
    }
}
