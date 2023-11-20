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

        public async Task<ResponseModel<UserProfile>> GetCurrentUserProfile(int id)
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

        public async Task<ResponseModel<UserProfile>> UpdateProfile(UpdateUserProfileRequest profile, int userId)
        {
            ResponseModel<UserProfile> response = new();
            try
            {
                var updatedProfile = await _context.UserProfiles.FirstOrDefaultAsync(x => x.UserId == userId);
                if (updatedProfile is not null)
                {
                    string filePath = $"{_environment.WebRootPath}\\{updatedProfile.UserId}";
                    string profileImagePath = $"{filePath}\\{DateTime.Now.ToString("dd.MM.yyyy.HH.mm.ss.ffffff")}.png";
                    string backImagePath = $"{filePath}\\{DateTime.Now.ToString("dd.MM.yyyy.HH.mm.ss.ffffff")}.png";
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    if (File.Exists(profileImagePath))
                    {
                        File.Delete(profileImagePath);
                    }
                    if (File.Exists(backImagePath))
                    {
                        File.Delete(backImagePath);
                    }
                    if (profile.ProfilePicture is not null)
                    {
                        using (FileStream stream = File.Create(profileImagePath))
                        {
                            await profile.ProfilePicture!.CopyToAsync(stream);
                        }
                        updatedProfile.ProfilePicture = profileImagePath;
                    }
                    if(profile.BackPicture is not null)
                    {
                        using (FileStream stream = File.Create(backImagePath))
                        {
                            await profile.BackPicture!.CopyToAsync(stream);
                        }
                        updatedProfile.BackPicture = backImagePath;
                    }

                    updatedProfile.UserName = profile.UserName;
                    updatedProfile.FullName = profile.FullName;
                    updatedProfile.Bio = profile.Bio;
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
