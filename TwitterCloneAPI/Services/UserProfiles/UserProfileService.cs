using Microsoft.EntityFrameworkCore;
using TwitterCloneAPI.Models;
using TwitterCloneAPI.Models.ServiceResponse;
using TwitterCloneAPI.Models.UserProfileRequest;

namespace TwitterCloneAPI.Services.UserProfiles
{
    public class UserProfileService : IUserProfileService
    {
        private readonly TwitterCloneContext _context;
        private readonly IWebHostEnvironment _environment;
        public UserProfileService(TwitterCloneContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;

        }

        public async Task<ResponseModel<UserProfile>> GetProfileByUserId(int id)
        {
            ResponseModel<UserProfile> response = new();
            try
            {
                response.Data = await _context.UserProfiles.FirstAsync(x => x.UserId == id);
                response.Success = true;
                response.Message = "Profile found";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseModel<UserProfile>> UpdateProfile(UpdateUserFrofileRequest profile, int userId)
        {
            ResponseModel<UserProfile> response = new();
            try
            {
                var updatedProfile = await _context.UserProfiles.FirstOrDefaultAsync(x => x.UserId == userId);
                if (updatedProfile is not null)
                {
                    updatedProfile.UserName = profile.userName;
                    updatedProfile.FullName = profile.fullName;
                    updatedProfile.Bio = profile.bio;
                    await _context.SaveChangesAsync();
                    response.Data = updatedProfile;
                    response.Success = true;
                    response.Message = "Profile update!";
                }
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
