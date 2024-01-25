using TwitterCloneAPI.Models;
using TwitterCloneAPI.Models.HashtagResponse;
using TwitterCloneAPI.Models.ServiceResponse;

namespace TwitterCloneAPI.Services.HashtagsService
{
    public class HashtagService : IHashtagService
    {
        private readonly TwitterCloneContext _context;
        public HashtagService(TwitterCloneContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<int>> AddHashtags(string[] hashtags, int tweetId)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<List<HashtagResponseModel>>> GetHashtags()
        {
            throw new NotImplementedException();
        }
    }
}
