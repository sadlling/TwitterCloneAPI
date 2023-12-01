using Microsoft.EntityFrameworkCore;
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

        public async Task<ResponseModel<int>> AddTweetInRetweets(int userId, int tweetId)
        {
            var response = new ResponseModel<int>();
            try
            {
                //TODO check this tweet is not from this user??
                if (await _context.Retweets.AnyAsync(x => x.TweetId == tweetId && x.UserId == userId))
                {
                    response.Success = false;
                    response.Message = "This tweet exist in reweets!";
                    return response;
                }
                if (!await _context.Tweets.AnyAsync(x => x.TweetId == tweetId))
                {
                    response.Success = false;
                    response.Message = "This tweet is not exists!";
                    return response;
                }
                await _context.Retweets.AddAsync(new Retweet
                {
                    UserId = userId,
                    TweetId = tweetId,
                    CreatedAt = DateTime.Now,
                });
                await _context.SaveChangesAsync();
                response.Success = true;
                response.Message = "Tweet added in retweets";

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
         
        }

        public async Task<ResponseModel<int>> RemoveTweetFromRetweets(int userId, int tweetId)
        {
          var response = new ResponseModel<int>();
            try
            {
                if (!await _context.Tweets.AnyAsync(x => x.TweetId == tweetId))
                {
                    response.Success = false;
                    response.Message = "This tweet is not exists!";
                    return response;
                }
                var currentRetweet = await _context.Retweets.FirstOrDefaultAsync(x=> x.TweetId == tweetId && x.UserId == userId) ?? null;
                if (currentRetweet is null)
                {
                    response.Success = false;
                    response.Message = "This tweet is not exist in reweets!";
                    return response;
                }
                _context.Retweets.Remove(currentRetweet);
                await _context.SaveChangesAsync();
                response.Success = true;
                response.Message = "Tweet delete from retweets!";

            }
            catch (Exception ex)
            {
                response.Success = false; 
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
