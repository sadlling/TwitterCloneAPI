using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using TwitterCloneAPI.Models;
using TwitterCloneAPI.Models.ServiceResponse;
using TwitterCloneAPI.Models.TweetRequest;
using TwitterCloneAPI.Models.TweetResponse;
using TwitterCloneAPI.Services.Hashtags;

namespace TwitterCloneAPI.Services.Tweets
{
    public class TweetService : ITweetService
    {
        private readonly TwitterCloneContext _context;
        private readonly IHashtagService _hashtagService;
        public TweetService(TwitterCloneContext context, IHashtagService hashtagService)
        {
            _context = context;
            _hashtagService = hashtagService;
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
                var currentUser = await _context.UserProfiles.FirstAsync(x => x.UserId == userId);
                response.Data = new TweetResponseModel
                {
                    TweetId = newTweet.TweetId,
                    PostedUserId = newTweet.UserId,
                    PostedUserName = currentUser.FullName ?? currentUser.UserName ?? "",
                    PostedUserImage = currentUser.ProfilePicture!.Replace("\\", "/").Replace("wwwroot/", "") ?? "",
                    Content = newTweet.Content ?? " ",
                    Image = newTweet.TweetImage?.Replace("\\", "/").Replace("wwwroot/", "") ?? "",
                    IsPublic = newTweet.IsPublic,
                    CreatedAt = newTweet.CreateAt ?? DateTime.Now,
                    CommentsCount = newTweet.Comments.Count,
                    RetweetCount = newTweet.Retweets.Count,
                    LikesCount = newTweet.Likes.Count,
                    SaveCount = newTweet.SavedTweets.Count,
                    IsOwner = true,
                };
                response.Message = "Tweet Created!";
                response.Success = true;

                if (request.Hashtags is not null)
                {
                    var resultAddingHashtags = await _hashtagService.AddHashtags(request.Hashtags, newTweet.TweetId);
                    if (!resultAddingHashtags.Success)
                    {
                        response.Message += "Hashtags not added!";
                    }
                    else
                    {
                        response.Message += resultAddingHashtags.Message;
                    }
                }


            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }
            return response;
        }

        public async Task<ResponseModel<int>> DeleteTweet(int tweetId)
        {
            ResponseModel<int> response = new();
            try
            {
                var deletableTweet = await _context.Tweets.FirstOrDefaultAsync(x => x.TweetId == tweetId) ?? null;
                if (deletableTweet is not null)
                {
                    _context.Tweets.Remove(deletableTweet);
                    await _context.SaveChangesAsync();
                    response.Success = true;
                    response.Message = "Tweet deleted!";
                    return response;
                }
                response.Success = false;
                response.Message = "Tweet is not exist";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseModel<List<TweetResponseModel>>> GetAllTweets(int userId)
        {
            ResponseModel<List<TweetResponseModel>> response = new();
            try
            {
                response.Data = await _context.Tweets.Include(y => y.User).Select(x => new TweetResponseModel
                {
                    TweetId = x.TweetId,
                    PostedUserId = x.UserId,
                    PostedUserName = !string.IsNullOrEmpty(x.User.UserProfile!.FullName) ? x.User.UserProfile!.FullName : x.User.UserProfile!.UserName ?? "",
                    PostedUserImage = x.User.UserProfile!.ProfilePicture!.Replace("\\", "/").Replace("wwwroot/", "") ?? "",
                    Content = x.Content ?? " ",
                    Image = x.TweetImage!.Replace("\\", "/").Replace("wwwroot/", "") ?? "",
                    IsPublic = x.IsPublic,
                    CreatedAt = x.CreateAt ?? DateTime.Now,
                    CommentsCount = x.Comments.Count,
                    RetweetCount = x.Retweets.Count,
                    LikesCount = x.Likes.Count,
                    SaveCount = x.SavedTweets.Count,
                    IsOwner = x.UserId == userId

                }).ToListAsync();

                foreach (var item in response.Data)
                {
                    item.IsRetweeted = await _context.Retweets.AnyAsync(x => x.UserId == userId && x.TweetId == item.TweetId);
                    item.IsLiked = await _context.Likes.AnyAsync(x => x.UserId == userId && x.TweetId == item.TweetId);
                    item.IsSaved = await _context.SavedTweets.AnyAsync(x => x.UserId == userId && x.TweetId == item.TweetId);
                }

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

        public async Task<ResponseModel<List<TweetResponseModel>>> GetCurrentUserTweetsAndRetweets(int userId, int currentUserId)
        {
            ResponseModel<List<TweetResponseModel>> response = new();
            try
            {
                var retweets = await _context.Retweets.Include(p => p.User).Include(x => x.Tweet).Where(y => y.UserId == userId).Select(x => new TweetResponseModel
                {
                    TweetId = x.Tweet.TweetId,
                    PostedUserId = x.Tweet.UserId,
                    PostedUserName = !string.IsNullOrEmpty(x.User.UserProfile!.FullName) ? x.User.UserProfile!.FullName : x.User.UserProfile!.UserName ?? "",
                    PostedUserImage = x.User.UserProfile!.ProfilePicture!.Replace("\\", "/").Replace("wwwroot/", "") ?? "",
                    Content = x.Tweet.Content,
                    Image = x.Tweet.TweetImage!.Replace("\\", "/").Replace("wwwroot/", "") ?? "",
                    IsPublic = x.Tweet.IsPublic,
                    CreatedAt = x.CreatedAt ?? DateTime.Now,
                    CommentsCount = x.Tweet.Comments.Count,
                    RetweetCount = x.Tweet.Retweets.Count,
                    LikesCount = x.Tweet.Likes.Count,
                    SaveCount = x.Tweet.SavedTweets.Count,
                    IsRetweeted = true,
                }).ToListAsync();

                var userTweets = await _context.Tweets.Include(y => y.User).Where(x => x.UserId == userId).Select(x => new TweetResponseModel
                {
                    TweetId = x.TweetId,
                    PostedUserId = x.UserId,
                    PostedUserName = !string.IsNullOrEmpty(x.User.UserProfile!.FullName) ? x.User.UserProfile!.FullName : x.User.UserProfile!.UserName ?? "",
                    PostedUserImage = x.User.UserProfile!.ProfilePicture!.Replace("\\", "/").Replace("wwwroot/", "") ?? "",
                    Content = x.Content ?? " ",
                    Image = x.TweetImage!.Replace("\\", "/").Replace("wwwroot/", "") ?? "",
                    IsPublic = x.IsPublic,
                    CreatedAt = x.CreateAt ?? DateTime.Now,
                    CommentsCount = x.Comments.Count,
                    RetweetCount = x.Retweets.Count,
                    LikesCount = x.Likes.Count,
                    SaveCount = x.SavedTweets.Count,
                    IsOwner = x.UserId == currentUserId
                }).ToListAsync();

                response.Data = userTweets.Concat(retweets).OrderBy(x => x.CreatedAt).ToList();

                foreach (var item in response.Data)
                {
                    item.IsLiked = await _context.Likes.AnyAsync(x => x.UserId == currentUserId && x.TweetId == item.TweetId);
                    item.IsSaved = await _context.SavedTweets.AnyAsync(x => x.UserId == currentUserId && x.TweetId == item.TweetId);
                }

                if (response.Data.Count <= 0)
                {
                    response.Success = true;
                    response.Message = "No tweets(";
                    return response;
                }
                response.Success = true;
                response.Message = "UserTweets";

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseModel<List<TweetResponseModel>>> GetTweetsByHashtags(int userId, string[] hashtags)
        {
            ResponseModel<List<TweetResponseModel>> response = new();
            try
            {
                var hastagsIds = await _context.TweetHashtags.
                    Include(x => x.Tweet).
                    Include(x => x.Hashtag).
                    Where(x => hashtags.Select(y => y).Contains(x.Hashtag.Name)).Select(x => x.TweetId).
                    Distinct().
                    ToListAsync();
                var tweetsByHastags = await _context.Tweets.
                    Include(x => x.User.UserProfile).
                    Where(x => hastagsIds.Select(y => y).Contains(x.TweetId)).
                    ToListAsync();

                response.Data = new List<TweetResponseModel>(tweetsByHastags.Select(x => new TweetResponseModel
                {
                    TweetId = x.TweetId,
                    PostedUserId = x.UserId,
                    PostedUserName = !string.IsNullOrEmpty(x.User.UserProfile!.FullName) ? x.User.UserProfile!.FullName ?? "" : x.User.UserProfile!.UserName ?? "",
                    PostedUserImage = x.User.UserProfile!.ProfilePicture?.Replace("\\", "/").Replace("wwwroot/", "") ?? "",
                    Content = x.Content ?? " ",
                    Image = x.TweetImage?.Replace("\\", "/").Replace("wwwroot/", "") ?? "",
                    IsPublic = x.IsPublic,
                    CreatedAt = x.CreateAt ?? DateTime.Now,
                    CommentsCount = x.Comments.Count,
                    RetweetCount = x.Retweets.Count,
                    LikesCount = x.Likes.Count,
                    SaveCount = x.SavedTweets.Count,
                    IsOwner = x.UserId == userId
                }).ToList());

                foreach (var item in response.Data)
                {
                    item.IsRetweeted = await _context.Retweets.AnyAsync(x => x.UserId == userId && x.TweetId == item.TweetId);
                    item.IsLiked = await _context.Likes.AnyAsync(x => x.UserId == userId && x.TweetId == item.TweetId);
                    item.IsSaved = await _context.SavedTweets.AnyAsync(x => x.UserId == userId && x.TweetId == item.TweetId);
                }

                response.Success = true;
                response.Message = "Tweets by hashtags";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseModel<TweetResponseModel>> UpdateTweet(UpdateTweetRequestModel request, int userId, int tweetId)
        {
            var response = new ResponseModel<TweetResponseModel>();
            try
            {
                var updatedTweet = await _context.Tweets.Include(y => y.User).FirstOrDefaultAsync(x => x.TweetId == tweetId) ?? null;
                if (updatedTweet is not null)
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
                    if (!string.IsNullOrEmpty(request.OldTweetImage) && request.NewTweetImage is null)
                    {
                        if (!request.OldTweetImage.Contains(updatedTweet.TweetImage?.Replace("\\", "/").Replace("wwwroot/", "") ?? ""))
                        {
                            updatedTweet.TweetImage = null;
                        }
                    }
                    if (request.NewTweetImage is not null)
                    {
                        using (FileStream stream = File.Create(tweetImagePath))
                        {
                            await request.NewTweetImage!.CopyToAsync(stream);
                        }
                        if (File.Exists(updatedTweet.TweetImage))
                        {
                            File.Delete(updatedTweet.TweetImage);
                        }
                        updatedTweet.TweetImage = tweetImagePath;
                    }

                    updatedTweet.Content = request.Content;
                    updatedTweet.IsPublic = request.IsPublic;
                    updatedTweet.UpdatedAt = DateTime.Now;
                    await _context.SaveChangesAsync();
                    var currentUser = await _context.UserProfiles.FirstAsync(x => x.UserId == userId);
                    response.Data = new TweetResponseModel
                    {
                        TweetId = updatedTweet.TweetId,
                        PostedUserId = updatedTweet.UserId,
                        PostedUserName = currentUser.FullName ?? currentUser.UserName ?? "",
                        PostedUserImage = currentUser.ProfilePicture!.Replace("\\", "/").Replace("wwwroot/", "") ?? "",
                        Content = updatedTweet.Content ?? " ",
                        Image = updatedTweet.TweetImage?.Replace("\\", "/").Replace("wwwroot/", "") ?? "",
                        IsPublic = updatedTweet.IsPublic,
                        CreatedAt = updatedTweet.CreateAt ?? DateTime.Now,
                        CommentsCount = updatedTweet.Comments.Count,
                        RetweetCount = updatedTweet.Retweets.Count,
                        LikesCount = updatedTweet.Likes.Count,
                        SaveCount = updatedTweet.SavedTweets.Count,
                        IsRetweeted = await _context.Retweets.AnyAsync(x => x.UserId == userId && x.TweetId == updatedTweet.TweetId),
                        IsLiked = await _context.Likes.AnyAsync(x => x.UserId == userId && x.TweetId == updatedTweet.TweetId),
                        IsSaved = await _context.SavedTweets.AnyAsync(x => x.UserId == userId && x.TweetId == updatedTweet.TweetId),
                        IsOwner = true,
                    };
                    if (request.Hashtags is not null)
                    {
                        var resultAddingHashtags = await _hashtagService.AddHashtags(request.Hashtags, updatedTweet.TweetId);
                        if (!resultAddingHashtags.Success)
                        {
                            response.Message += "Hashtags not added!";
                        }
                        else
                        {
                            response.Message += resultAddingHashtags.Message;
                        }
                    }

                    response.Message += "Tweet Updated!";
                    response.Success = true;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Message += ex.StackTrace;
            }
            return response;
        }
    }
}
