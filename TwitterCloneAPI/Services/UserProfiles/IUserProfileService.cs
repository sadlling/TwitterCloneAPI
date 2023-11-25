using TwitterCloneAPI.Models.ServiceResponse;
using TwitterCloneAPI.Models;
using TwitterCloneAPI.Models.UserProfileRequest;
using TwitterCloneAPI.Models.UserResponse;

namespace TwitterCloneAPI.Services.UserProfiles
{
    public interface IUserProfileService
    {
        public Task<ResponseModel<UserResponseModel>> UpdateProfile(UpdateUserProfileRequest profile,int userId);
        public Task<ResponseModel<UserResponseModel>> GetProfileByUserId(int  id);
        public Task<ResponseModel<UserResponseModel>> GetCurrentUserProfile(int id);

    }
}
