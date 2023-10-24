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
        public UserService(TwitterCloneContext context,ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }
        public async Task<ResponseModel<string>> CreateUser(UserRequestModel newUser)
        {
            var response = new ResponseModel<string> ();
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
                user = _context.UserAuthentications.FirstOrDefault(x => x.Email.Equals(request.Email))!;
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

        public async Task<ResponseModel<UserProfile>> GetUserById(int id)
        {
            UserProfile user;
            var response = new ResponseModel<UserProfile>();
            try
            {
                user = _context.UserProfiles.FirstOrDefault(x => x.ProfileId == id)!;
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
    }
}
