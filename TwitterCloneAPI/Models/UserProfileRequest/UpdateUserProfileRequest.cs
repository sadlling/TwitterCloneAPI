namespace TwitterCloneAPI.Models.UserProfileRequest
{
    public class UpdateUserProfileRequest
    {
        public string? UserName {  get; set; } = string.Empty;
        public string? FullName { get; set; } = string.Empty;
        public string? Bio { get; set; } = string.Empty;
        public IFormFile? ProfilePicture { get; set; } = null!;
        public IFormFile? BackPicture { get; set; } = null!;

    }
}
