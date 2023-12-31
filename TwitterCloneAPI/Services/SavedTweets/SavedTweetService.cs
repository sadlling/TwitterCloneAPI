﻿using Microsoft.EntityFrameworkCore;
using TwitterCloneAPI.Models.ServiceResponse;
using TwitterCloneAPI.Models;
using TwitterCloneAPI.Models.TweetResponse;

namespace TwitterCloneAPI.Services.SavedTweets
{
    public class SavedTweetService : ISavedTweetSevice
    {
        private readonly TwitterCloneContext _context;
        public SavedTweetService(TwitterCloneContext context)
        {
            _context = context;
        }
        public async Task<ResponseModel<int>> AddTweetInSaved(int userId, int tweetId)
        {

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

        public async Task<ResponseModel<int>> DeleteTweetInSaved(int userId, int tweetId)
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
        public async Task<ResponseModel<List<TweetResponseModel>>> GetSavedTweets(int userId)
        {
            ResponseModel<List<TweetResponseModel>> response = new();
            try
            {
                response.Data = new List<TweetResponseModel>();
                var savedTweets = await _context.SavedTweets.Where(x => x.UserId == userId).ToListAsync();
                if (savedTweets.Count <= 0)
                {
                    response.Success = true;
                    response.Message = "No saved tweets(";
                    return response;
                }
                foreach (var savedTweet in savedTweets)
                {
                    response.Data.Add(await GetTweetByTweetIdAsync(savedTweet.TweetId));
                }
                response.Success = true;
                response.Message = "All saved tweets";
            }
            catch (Exception ex)
            {
                response.Data = null;
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
        private async Task<TweetResponseModel> GetTweetByTweetIdAsync(int tweetId)
        {
            return await _context.Tweets
                .Where(y => y.TweetId == tweetId).Select(x => new TweetResponseModel
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

                }).FirstAsync();
        }
    }
}
