using Microsoft.EntityFrameworkCore;
using TwitterCloneAPI.Models;
using TwitterCloneAPI.Models.ServiceResponse;
using TwitterCloneAPI.Models.UserProfileRequest;
using TwitterCloneAPI.Models.UserResponse;

namespace TwitterCloneAPI.Services.UserProfiles
{
    public class UserProfileService : IUserProfileService
    {
        private readonly TwitterCloneContext _context;

        public UserProfileService(TwitterCloneContext context)
        {
            _context = context;

        }

        public async Task<ResponseModel<UserResponseModel>> GetCurrentUserProfile(int userId)
        {
            ResponseModel<UserResponseModel> response = new();
            try
            {
                var user = await _context.UserAuthentications
                    .Include(x => x.UserProfile)
                    .Include(x => x.FollowerUsers)
                    .Include(x => x.FollowerFollowerUsers)
                    .AsSplitQuery()
                    .FirstAsync(x => x.UserId == userId);
                response.Data = new UserResponseModel
                {
                    UserId = user.UserId,
                    UserEmail = user.Email,
                    UserName = user.UserProfile?.UserName ?? "",
                    FullName = user.UserProfile?.FullName ?? "",
                    ProfilePicture = user.UserProfile!.ProfilePicture?.Replace("\\", "/").Replace("wwwroot/", "") ?? "",
                    BackPicture = user.UserProfile!.BackPicture?.Replace("\\", "/").Replace("wwwroot/", "") ?? "",
                    QuantityOfFollowers = user.FollowerUsers.Count(),
                    QuantityOfFollowing = user.FollowerFollowerUsers.Count(),
                    ProfileDescription = user.UserProfile?.Bio ?? ""
                };

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

        public async Task<ResponseModel<UserResponseModel>> GetProfileByUserId(int userId,int currentUserId)
        {
            ResponseModel<UserResponseModel> response = new();
            try
            {
                var user = await _context.UserAuthentications
                  .Include(x => x.UserProfile)
                  .Include(x => x.FollowerUsers)
                  .Include(x => x.FollowerFollowerUsers)
                  .AsSplitQuery()
                  .FirstAsync(x => x.UserId == userId);

                response.Data = new UserResponseModel
                {
                    UserId = user.UserId,
                    UserEmail = user.Email,
                    UserName = user.UserProfile?.UserName ?? "",
                    FullName = user.UserProfile?.FullName ?? "",
                    ProfilePicture = user.UserProfile!.ProfilePicture?.Replace("\\", "/").Replace("wwwroot/", "") ?? "",
                    BackPicture = user.UserProfile!.BackPicture?.Replace("\\", "/").Replace("wwwroot/", "") ?? "",
                    QuantityOfFollowers = user.FollowerUsers.Count(),
                    QuantityOfFollowing = user.FollowerFollowerUsers.Count(),
                    ProfileDescription = user.UserProfile?.Bio ?? "",
                    IsSubscribed = await _context.Followers.AnyAsync(x=>x.UserId==userId && x.FollowerUserId ==currentUserId) 
                };
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

        public async Task<ResponseModel<List<UserResponseModel>>> GetTwoPopularProfiles(int currentUserId)
        {
            ResponseModel<List<UserResponseModel>> response = new();
            try
            {
                var users = await _context.UserAuthentications
                 .Include(x => x.UserProfile)
                 .Include(x => x.FollowerUsers)
                 .Include(x => x.FollowerFollowerUsers)
                 .AsSplitQuery()
                 .OrderByDescending(x => x.FollowerUsers.Count())
                 .Take(2).ToListAsync();

                response.Data = users.Select(x =>new UserResponseModel
                {
                    UserId = x.UserId,
                    UserEmail = x.Email,
                    UserName = x.UserProfile?.UserName ?? "",
                    FullName = x.UserProfile?.FullName ?? "",
                    ProfilePicture = x.UserProfile!.ProfilePicture?.Replace("\\", "/").Replace("wwwroot/", "") ?? "",
                    BackPicture = x.UserProfile!.BackPicture?.Replace("\\", "/").Replace("wwwroot/", "") ?? "",
                    QuantityOfFollowers = x.FollowerUsers.Count(),
                    QuantityOfFollowing = x.FollowerFollowerUsers.Count(),
                    ProfileDescription = x.UserProfile?.Bio ?? "",
                    IsSubscribed = _context.Followers.Any(y => y.UserId == x.UserId && y.FollowerUserId == currentUserId)
                }).ToList();

                response.Success = true;
                response.Message = "Profiles found";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message; 
            }
            return response;
        }

        public async Task<ResponseModel<UserResponseModel>> UpdateProfile(UpdateUserProfileRequest profile, int userId)
        {
            ResponseModel<UserResponseModel> response = new();
            try
            {
                var updatedProfile = await _context.UserAuthentications
                  .Include(x => x.UserProfile)
                  .Include(x => x.FollowerUsers)
                  .Include(x => x.FollowerFollowerUsers)
                  .AsSplitQuery()
                  .FirstAsync(x => x.UserId == userId);

                if (updatedProfile is not null)
                {
                    string filePath = $"wwwroot\\{updatedProfile.UserId}";
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
                        updatedProfile.UserProfile!.ProfilePicture = profileImagePath;
                    }
                    if (profile.BackPicture is not null)
                    {
                        using (FileStream stream = File.Create(backImagePath))
                        {
                            await profile.BackPicture!.CopyToAsync(stream);
                        }
                        updatedProfile.UserProfile!.BackPicture = backImagePath;
                    }

                    updatedProfile.UserProfile!.UserName = profile.UserName;
                    updatedProfile.UserProfile!.FullName = profile.FullName;
                    updatedProfile.UserProfile!.Bio = profile.Bio;
                    await _context.SaveChangesAsync();
                    response.Data = new UserResponseModel
                    {
                        UserId = updatedProfile.UserId,
                        UserEmail = updatedProfile.Email,
                        UserName = updatedProfile.UserProfile?.UserName ?? "",
                        FullName = updatedProfile.UserProfile?.FullName ?? "",
                        ProfilePicture = updatedProfile.UserProfile?.ProfilePicture!.Replace("\\", "/").Replace("wwwroot/", "") ?? "",
                        BackPicture = updatedProfile.UserProfile?.BackPicture!.Replace("\\", "/").Replace("wwwroot/", "") ?? "",
                        QuantityOfFollowers = updatedProfile.FollowerUsers.Count(),
                        QuantityOfFollowing = updatedProfile.FollowerFollowerUsers.Count(),
                        ProfileDescription = updatedProfile.UserProfile?.Bio ?? ""
                    };
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
