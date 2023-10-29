using TwitterCloneAPI.Models.ServiceResponse;
using TwitterCloneAPI.Models;
using TwitterCloneAPI.Models.UserProfileRequest;

namespace TwitterCloneAPI.Services.UserProfiles
{
    public interface IUserProfileService
    {
        public Task<ResponseModel<UserProfile>> UpdateProfile(UpdateUserFrofileRequest profile,int userId);
        public Task<ResponseModel<UserProfile>> GetProfileByUserId(int  id);

    }
}
