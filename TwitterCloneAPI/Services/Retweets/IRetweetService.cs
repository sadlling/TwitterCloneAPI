using TwitterCloneAPI.Models.ServiceResponse;
using TwitterCloneAPI.Models.TweetResponse;

namespace TwitterCloneAPI.Services.Retweets
{
    public interface IRetweetService
    {
        public Task<ResponseModel<List<TweetResponseModel>>> GetRetweetsByUserId(int userId);
    }
}
