using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using TwitterCloneAPI.Models;
using TwitterCloneAPI.Models.RefreshToken;
using TwitterCloneAPI.Models.ServiceResponse;
using TwitterCloneAPI.Models.UserRequest;
using TwitterCloneAPI.Services.Token;

namespace TwitterCloneAPI.Services.User
{
    public class UserService : IUserService
    {
        private readonly TwitterCloneContext _context;
        private readonly ITokenService _tokenService;
        public UserService(TwitterCloneContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }
        public async Task<ResponseModel<string>> CreateUser(UserRequestModel newUser)
        {
            var response = new ResponseModel<string>();
            UserAuthentication user = new();
            try
            {
                RefreshToken refreshToken = _tokenService.CreateRefreshToken();
                user.Email = newUser.Email;
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
                user.RefreshToken = refreshToken.Token;
                user.TokenCreated = DateTime.Now;
                user.TokenExpires = refreshToken.Expired;
                await _context.UserAuthentications.AddAsync(user);
                _context.SaveChanges();
                await _context.UserProfiles.AddAsync(new UserProfile
                {
                    UserId = user.UserId,
                    UserName = "User - " + RandomNumberGenerator.GetInt32(50000),
                    FullName = "",
                    Bio = "",
                    ProfilePicture = "",
                    BackPicture = "",
                    CreatedAt = DateTime.Now,
                });
                _context.SaveChanges();
                response.Success = true;
                response.Message = "Success registration!";

            }
            catch (Exception)
            {
                response.Success = false;
                response.Message = "User with this email already exists";
            }
            return response;

        }

        public async Task<ResponseModel<UserAuthentication>> GetUserByEmail(UserRequestModel request)
        {
            UserAuthentication user;
            var response = new ResponseModel<UserAuthentication>();
            try
            {
                user = await _context.UserAuthentications.Include(x => x.UserProfile).FirstAsync(x => x.Email.Equals(request.Email))!;
                response.Data = user;
                response.Success = true;
                response.Message = "User found!";

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseModel<UserAuthentication>> GetUserById(int id)
        {
            UserAuthentication user;
            var response = new ResponseModel<UserAuthentication>();
            try
            {
                user = await _context.UserAuthentications.FirstAsync(x => x.UserId == id)!;
                response.Data = user;
                response.Success = true;
                response.Message = "User found!";

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;

            }
            return response;
        }


        public async Task<ResponseModel<UserAuthentication>> UpdateUserRefreshToken(UserAuthentication request)
        {
            var response = new ResponseModel<UserAuthentication>();
            try
            {
                RefreshToken refreshToken = _tokenService.CreateRefreshToken();
                request.RefreshToken = refreshToken.Token;
                request.TokenCreated = refreshToken.Created;
                request.TokenExpires = refreshToken.Expired;
                _context.Entry(request).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                response.Data = request;
                response.Success = true;
                response.Message = "User updated";
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
