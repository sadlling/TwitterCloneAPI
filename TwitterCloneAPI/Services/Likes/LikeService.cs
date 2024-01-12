using Microsoft.EntityFrameworkCore;
using TwitterCloneAPI.Models;
using TwitterCloneAPI.Models.ServiceResponse;

namespace TwitterCloneAPI.Services.Likes
{
    public class LikeService : ILikeService
    {
        private readonly TwitterCloneContext _context;
        public LikeService(TwitterCloneContext context)
        {
            _context = context;
        }
        public async Task<ResponseModel<int>> AddTweetInLiked(int userId, int tweetId)
        {
            var response = new ResponseModel<int>();
            try
            {
                if (await _context.Likes.AnyAsync(x => x.TweetId == tweetId && x.UserId == userId))
                {
                    response.Success = false;
                    response.Message = "This tweet exist in liked tweets!";
                    return response;
                }
                if (!await _context.Tweets.AnyAsync(x => x.TweetId == tweetId))
                {
                    response.Success = false;
                    response.Message = "This tweet is not exists!";
                    return response;
                }
                await _context.Likes.AddAsync(new Like
                {
                    UserId = userId,
                    TweetId = tweetId,
                    CreatedAt = DateTime.Now,
                });
                await _context.SaveChangesAsync();
                response.Success = true;
                response.Message = "Tweet added in liked tweets!";

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseModel<int>> DeleteTweetFromLiked(int userId, int tweetId)
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
                var currentLikedTweet = await _context.Likes.FirstOrDefaultAsync(x => x.TweetId == tweetId && x.UserId == userId) ?? null;
                if (currentLikedTweet is null)
                {
                    response.Success = false;
                    response.Message = "This tweet is not exist in liked tweets!";
                    return response;
                }
                _context.Likes.Remove(currentLikedTweet);
                await _context.SaveChangesAsync();
                response.Success = true;
                response.Message = "Tweet delete from liked tweets!";

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
