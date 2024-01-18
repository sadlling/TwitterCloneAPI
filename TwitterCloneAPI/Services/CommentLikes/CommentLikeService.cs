using Microsoft.EntityFrameworkCore;
using TwitterCloneAPI.Models;
using TwitterCloneAPI.Models.ServiceResponse;

namespace TwitterCloneAPI.Services.CommentLikes
{
    public class CommentLikeService : ICommentLikeService
    {
        private readonly TwitterCloneContext _context;
        public CommentLikeService(TwitterCloneContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<int>> AddLikeOnComment(int userId, int commentId)
        {
            var response = new ResponseModel<int>();
            try
            {
                if (!await _context.Comments.AnyAsync(x => x.CommentId == commentId))
                {
                    response.Success = false;
                    response.Message = "This comment is not exists!";
                    return response;
                }
                if (await _context.CommentLike.AnyAsync(x => x.CommentId == commentId && x.UserId == userId))
                {
                    response.Success = false;
                    response.Message = "This comment is liked!";
                    return response;
                }
                await _context.CommentLike.AddAsync(new CommentLike
                {
                    UserId = userId,
                    CommentId = commentId,
                    CreatedAt = DateTime.Now,
                });
                await _context.SaveChangesAsync();
                response.Success = true;
                response.Message = "Comment liked!";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseModel<int>> DeleteLikeFromComment(int userId, int commentId)
        {
            var response = new ResponseModel<int>();
            try
            {
                if (!await _context.Comments.AnyAsync(x => x.CommentId == commentId))
                {
                    response.Success = false;
                    response.Message = "This comment is not exists!";
                    return response;
                }
                var currentCommentLike = await _context.CommentLike.FirstOrDefaultAsync(x => x.CommentId == commentId && x.UserId == userId) ?? null;
                if (currentCommentLike is null)
                {
                    response.Success = false;
                    response.Message = "This comment not liked!";
                    return response;
                }
                _context.CommentLike.Remove(currentCommentLike);
                await _context.SaveChangesAsync();
                response.Success = true;
                response.Message = "Like deleted!";

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
