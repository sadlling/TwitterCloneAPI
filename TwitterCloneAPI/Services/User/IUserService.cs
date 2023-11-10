using TwitterCloneAPI.Models;
using TwitterCloneAPI.Models.ServiceResponse;
using TwitterCloneAPI.Models.UserRequest;

namespace TwitterCloneAPI.Services.User
{
    public interface IUserService
    {
        public Task<ResponseModel<string>> CreateUser(UserRequestModel newUser);
        public Task<ResponseModel<UserAuthentication>> GetUserById(int id);
        public Task<ResponseModel<UserAuthentication>> GetUserByEmail(UserRequestModel request);
        public Task<ResponseModel<UserAuthentication>> UpdateUserRefreshToken(UserAuthentication request);
    }
}
