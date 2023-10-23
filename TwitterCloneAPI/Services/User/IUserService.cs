using TwitterCloneAPI.Models;
using TwitterCloneAPI.Models.ServiceResponse;
using TwitterCloneAPI.Models.UserRequest;

namespace TwitterCloneAPI.Services.User
{
    public interface IUserService
    {
        public Task<ResponseModel<string>> CreateUser(UserRequestModel newUser);
        public Task<ResponseModel<UserProfile>> GetUserById(int id);
    }
}
