using Microsoft.EntityFrameworkCore;
using System.IO.Pipes;
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


        public async Task<ResponseModel<Tweet>> CreateTweet(TweetRequestModel request, int userId)
        {
            ResponseModel<Tweet> response = new();
            Tweet newTweet = new();
            try
            {
                string filePath = $"{_environment.WebRootPath}\\{userId}";
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
                response.Data = newTweet;
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
                    Image = x.TweetImage ?? " ",
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
    }
}
