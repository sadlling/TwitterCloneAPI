namespace TwitterCloneAPI.Models.UserProfileRequest
{
    public class UpdateUserFrofileRequest
    {
        public string userName {  get; set; } = string.Empty;
        public string fullName { get; set; } = string.Empty;
        public string bio { get; set; } = string.Empty;
        public IFormFile profilePicture { get; set; } = null!;
        public IFormFile backPicture { get; set; } = null!;

    }
}
