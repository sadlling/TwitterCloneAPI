using Microsoft.EntityFrameworkCore;
using TwitterCloneAPI.Models;
using TwitterCloneAPI.Models.CommentRequest;
using TwitterCloneAPI.Models.CommentResponse;
using TwitterCloneAPI.Models.ServiceResponse;
using TwitterCloneAPI.Models.TweetResponse;

namespace TwitterCloneAPI.Services.Comments
{
    public class CommentService : ICommentService
    {
        private readonly TwitterCloneContext _context;
        public CommentService(TwitterCloneContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<CommentResponseModel>> CreateComment(CommentRequestModel request, int userId, int tweetId)
        {
            var response = new ResponseModel<CommentResponseModel>();
            Comment newComment = new();
            try
            {
                string filePath = $"wwwroot\\{userId}";
                string commentImagePath = $"{filePath}\\{DateTime.Now.ToString("dd.MM.yyyy.HH.mm.ss.ffffff")}.png";
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                if (File.Exists(commentImagePath))
                {
                    File.Delete(commentImagePath);
                }
                if (request.Image is not null)
                {
                    using (FileStream stream = File.Create(commentImagePath))
                    {
                        await request.Image!.CopyToAsync(stream);
                    }
                    newComment.CommentImage = commentImagePath;
                }
                newComment.UserId = userId;
                newComment.TweetId = tweetId;
                newComment.Content = request.Content;
                newComment.CreatedAt = DateTime.Now;
                newComment.UpdatedAt = DateTime.Now;

                await _context.Comments.AddAsync(newComment);
                await _context.SaveChangesAsync();

                response.Data = new CommentResponseModel
                {
                    CommentId = newComment.CommentId,
                    PosterUserId = newComment.UserId,
                    Content = newComment.Content,
                    Image = newComment.CommentImage?.Replace("\\", "/").Replace("wwwroot/", "") ?? "",
                    CreatedAt = newComment.CreatedAt ?? DateTime.Now,
                    UpdatedAt = newComment.UpdatedAt ?? DateTime.Now,
                };
                response.Message = "Comment Created!";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }
            return response;
        }

        public Task<ResponseModel<CommentResponseModel>> GetComment(int commentId)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseModel<List<CommentResponseModel>>> GetTweetComments(int tweetId)
        {
            var response = new ResponseModel<List<CommentResponseModel>>();
            try
            {
                if(!await _context.Tweets.AnyAsync(x=>x.TweetId==tweetId))
                {
                    response.Message = "Tweet not exist!";
                    response.Success = false;
                    return response;
                }
                response.Data = await _context.Comments.Where(y=>y.TweetId== tweetId).Select(x=> new CommentResponseModel
                {
                    CommentId = x.CommentId,
                    PosterUserId = x.UserId,
                    Content = x.Content ?? "",
                    Image = x.CommentImage!.Replace("\\", "/").Replace("wwwroot/", "") ?? "",
                    CreatedAt = x.CreatedAt ?? DateTime.Now,
                    UpdatedAt = x.UpdatedAt ?? DateTime.Now,

                }).ToListAsync();
                response.Message = "Tweet comments";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public Task<ResponseModel<CommentResponseModel>> UpdateComment(CommentRequestModel request, int userId, int commentId)
        {
            throw new NotImplementedException();
        }
    }
}
