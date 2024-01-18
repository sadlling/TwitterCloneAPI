using Microsoft.EntityFrameworkCore;
using TwitterCloneAPI.Models;
using TwitterCloneAPI.Models.ServiceResponse;
using TwitterCloneAPI.Models.TweetResponse;

namespace TwitterCloneAPI.Services.Folowers
{
    public class FolowerService : IFolowerService
    {
        private readonly TwitterCloneContext _context;
        public FolowerService(TwitterCloneContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<int>> AddFollower(int userId, int followerId)
        {
            var response = new ResponseModel<int>();
            try
            {
                if (!await _context.UserAuthentications.AnyAsync(x=>x.UserId == followerId))
                {
                    response.Success = false;
                    response.Message = "This folower not exists!";
                    return response;
                }
                if(await _context.Followers.AnyAsync(x=>x.UserId==followerId && x.FollowerUserId == userId))
                {
                    response.Success = false;
                    response.Message = "Subscription exists!";
                    return response;
                }
                await _context.Followers.AddAsync(new Follower()
                {
                    UserId = followerId,
                    FollowerUserId = userId,
                    CreatedAt = DateTime.Now,
                });
                await _context.SaveChangesAsync();
                response.Success = true;
                response.Message = "Subscribed!";

            }
            catch (Exception ex)
            {
                response.Data = 0;
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseModel<int>> RemoveFollower(int userId, int followerId)
        {
            var response = new ResponseModel<int>();
            try
            {
                if (!await _context.UserAuthentications.AnyAsync(x => x.UserId == followerId))
                {
                    response.Success = false;
                    response.Message = "This folower not exists!";
                    return response;
                }
                var subscribe = await _context.Followers.FirstOrDefaultAsync(x => x.UserId == followerId && x.FollowerUserId == userId) ?? null;
                if (subscribe is null)
                {
                    response.Success = false;
                    response.Message = "Subscription not exists!";
                    return response;
                }
                _context.Followers.Remove(subscribe);
                await _context.SaveChangesAsync();

                response.Success = true;
                response.Message = "Subscribe removed!";

            }
            catch (Exception ex)
            {
                response.Data = 0;
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
                    followersTweets.AddRange(await GetTweetsByUserIdAsync(follower.FollowerUserId));
                }

                response.Data = followersTweets;

                foreach (var item in response.Data)
                {
                    item.IsRetweeted = await _context.Retweets.AnyAsync(x => x.UserId == userId && x.TweetId == item.TweetId);
                    item.IsLiked = await _context.Likes.AnyAsync(x => x.UserId == userId && x.TweetId == item.TweetId);
                    item.IsSaved = await _context.SavedTweets.AnyAsync(x => x.UserId == userId && x.TweetId == item.TweetId);
                    item.IsOwner = item.PostedUserId == userId;
                }

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
        private async Task<List<TweetResponseModel>> GetTweetsByUserIdAsync(int userId)
        {
            return await _context.Tweets.Include(p=>p.User)
                .Where(y => y.UserId == userId).Select(x => new TweetResponseModel
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

                }).ToListAsync();
        }
    }
}
