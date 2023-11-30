using TwitterCloneAPI.Models;
using TwitterCloneAPI.Models.ServiceResponse;
using TwitterCloneAPI.Models.TweetResponse;

namespace TwitterCloneAPI.Services.Retweets
{
    public class RetweetService : IRetweetService
    {
        private readonly TwitterCloneContext _context;
        public RetweetService(TwitterCloneContext context)
        {
            _context = context;
        }

        public Task<ResponseModel<List<TweetResponseModel>>> GetRetweetsByUserId(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
