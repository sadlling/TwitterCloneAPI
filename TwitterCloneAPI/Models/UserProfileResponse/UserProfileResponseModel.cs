namespace TwitterCloneAPI.Models.UserProfileResponse
{
    public class UserProfileResponseModel
    {
        public UserProfile Profile { get; set; } = null!;
        public byte[] ProfilePicture { get; set; } = null!;
        public byte[] BackPicture { get; set; } = null!;


    }
}
