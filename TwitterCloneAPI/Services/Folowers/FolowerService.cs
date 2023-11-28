using Microsoft.EntityFrameworkCore;
using TwitterCloneAPI.Models;
using TwitterCloneAPI.Models.ServiceResponse;

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
    }
}
