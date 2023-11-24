using Microsoft.EntityFrameworkCore;
using TwitterCloneAPI.Models;
using TwitterCloneAPI.Models.ServiceResponse;
using TwitterCloneAPI.Models.TweetRequest;
using TwitterCloneAPI.Models.TweetResponse;

namespace TwitterCloneAPI.Services.Tweets
{
    public class TweetService : ITweetService
    {
        private readonly TwitterCloneContext _context;
        private readonly IWebHostEnvironment _environment;
        public TweetService(TwitterCloneContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;

        }

        public async Task<ResponseModel<int>> AddTweetInSaved(int tweetId, int userId)
        {
            ResponseModel<int> response = new();
            try
            {
                var currentTweet = await _context.Tweets.FirstAsync(x => x.TweetId == tweetId);

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

        public async Task<ResponseModel<TweetResponseModel>> CreateTweet(TweetRequestModel request, int userId)
        {
            ResponseModel<TweetResponseModel> response = new();
            Tweet newTweet = new();
            try
            {
                string filePath = $"wwwroot\\{userId}";
                string tweetImagePath = $"{filePath}\\{DateTime.Now.ToString("dd.MM.yyyy.HH.mm.ss.ffffff")}.png";
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                if (File.Exists(tweetImagePath))
                {
                    File.Delete(tweetImagePath);
                }
                if (request.TweetImage is not null)
                {
                    using (FileStream stream = File.Create(tweetImagePath))
                    {
                        await request.TweetImage!.CopyToAsync(stream);
                    }
                    newTweet.TweetImage = tweetImagePath;
                }
                newTweet.UserId = userId;
                newTweet.Content = request.Content;
                newTweet.IsPublic = request.IsPublic;
                newTweet.CreateAt = DateTime.Now;
                newTweet.UpdatedAt = DateTime.Now;
                await _context.Tweets.AddAsync(newTweet);
                await _context.SaveChangesAsync();
                response.Data = new TweetResponseModel
                {
                    TweetId = newTweet.TweetId,
                    PostedUserId = newTweet.UserId,
                    Content = newTweet.Content ?? " ",
                    Image = newTweet.TweetImage!.Replace("\\", "/").Replace("wwwroot/", "") ?? "",
                    IsPublic = newTweet.IsPublic,
                    CreatedAt = newTweet.CreateAt ?? DateTime.Now,
                    CommentsCount = newTweet.Comments.Count,
                    RetweetCount = newTweet.Retweets.Count,
                    LikesCount = newTweet.Likes.Count,
                    SaveCount = newTweet.SavedTweets.Count,
                };
                response.Message = "Tweet Created!";
                response.Success = true;

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }
            return response;
        }

        public async Task<ResponseModel<List<TweetResponseModel>>> GetAllTweets()
        {
            ResponseModel<List<TweetResponseModel>> response = new();
            try
            {
                response.Data = await _context.Tweets.Select(x => new TweetResponseModel
                {
                    TweetId = x.TweetId,
                    PostedUserId = x.UserId,
                    Content = x.Content ?? " ",
                    Image = x.TweetImage!.Replace("\\", "/").Replace("wwwroot/", "") ?? "",
                    IsPublic = x.IsPublic,
                    CreatedAt = x.CreateAt ?? DateTime.Now,
                    CommentsCount = x.Comments.Count,
                    RetweetCount = x.Retweets.Count,
                    LikesCount = x.Likes.Count,
                    SaveCount = x.SavedTweets.Count,
                }).ToListAsync();

                response.Success = true;
                response.Message = "All tweets";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;

        }

        public async Task<ResponseModel<List<TweetResponseModel>>> GetFollowersTweets(int userId)
        {
            ResponseModel<List<TweetResponseModel>> response = new();

            try
            {
                var followersTweets = new List<TweetResponseModel>();
                var followersList = await _context.Followers.Where(x => x.UserId == userId).ToListAsync();

                foreach (var follower in followersList)
                {
                    followersTweets.AddRange(await GetTweetsAsync(follower.FollowerUserId));
                }

                response.Data = followersTweets;
                response.Success = true;
                response.Message = "Followers tweets founded!";

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;

        }
        private async Task<List<TweetResponseModel>> GetTweetsAsync(int userId)
        {
            return await _context.Tweets
                .Where(y => y.UserId == userId).Select(x => new TweetResponseModel
                {
                    TweetId = x.TweetId,
                    PostedUserId = x.UserId,
                    Content = x.Content ?? " ",
                    Image = x.TweetImage!.Replace("\\", "/").Replace("wwwroot/", "") ?? "",
                    IsPublic = x.IsPublic,
                    CreatedAt = x.CreateAt ?? DateTime.Now,
                    CommentsCount = x.Comments.Count,
                    RetweetCount = x.Retweets.Count,
                    LikesCount = x.Likes.Count,
                    SaveCount = x.SavedTweets.Count,

                }).ToListAsync();

        }
    }
}
