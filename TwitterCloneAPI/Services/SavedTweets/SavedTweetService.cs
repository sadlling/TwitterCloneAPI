using Microsoft.EntityFrameworkCore;
using TwitterCloneAPI.Models.ServiceResponse;
using TwitterCloneAPI.Models;

namespace TwitterCloneAPI.Services.SavedTweets
{
    public class SavedTweetService : ISavedTweetSevice
    {
        private readonly TwitterCloneContext _context;
        public SavedTweetService(TwitterCloneContext context)
        {
            _context = context;
        }
        public async Task<ResponseModel<int>> AddTweetInSaved(int tweetId, int userId)
        {
            //TODO check this tweet is not from this user

            ResponseModel<int> response = new();
            try
            {
                if (await _context.SavedTweets.AnyAsync(x => x.UserId == userId && x.TweetId == tweetId))
                {
                    response.Success = false;
                    response.Message = "This tweet exists in saved";
                    return response;
                }

                var currentTweet = await _context.Tweets.FirstOrDefaultAsync(x => x.TweetId == tweetId) ?? null;

                if (currentTweet == null)
                {
                    response.Success = false;
                    response.Message = "This tweet not exists";
                    return response;
                }
                await _context.SavedTweets.AddAsync(new SavedTweet
                {
                    TweetId = tweetId,
                    UserId = userId,
                    CreatedAt = DateTime.Now,
                });
                await _context.SaveChangesAsync();

                response.Data = currentTweet.SavedTweets.Count;
                response.Success = true;
                response.Message = "Tweet aded in bookmarks";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }
            return response;
        }

        public async Task<ResponseModel<int>> DeleteTweetInSaved(int tweetId, int userId)
        {
            ResponseModel<int> response = new();
            try
            {
                
                var currentTweet = await _context.Tweets.FirstOrDefaultAsync(x => x.TweetId == tweetId) ?? null;

                if (currentTweet is null)
                {
                    response.Success = false;
                    response.Message = "This tweet not exists!";
                    return response;
                }
                var savedTweet = await _context.SavedTweets.FirstOrDefaultAsync(x => x.UserId == userId && x.TweetId == tweetId) ?? null;
                if (savedTweet is null)
                {
                    response.Success = false;
                    response.Message = "This tweet not exists in saved!";
                    return response;
                }
                _context.SavedTweets.Remove(savedTweet);
                await _context.SaveChangesAsync();

                response.Data = currentTweet.SavedTweets.Count;
                response.Success = true;
                response.Message = "Tweet delete from saved!";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }
            return response;
        }
    }
}
