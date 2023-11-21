namespace TwitterCloneAPI.Models.UserResponse
{
    public class UserResponseModel
    {
        public int UserID {  get; set; }
        public string UserEmail { get; set; } = string.Empty!;
        public string UserName { get; set; } = string.Empty!;
        public string FullName { get; set; } = string.Empty!;
        public string ProfilePicture { get; set; } = string.Empty;
        public string BackPicture {  get; set; } = string.Empty;    
        public int QuantityOfFollowers { get; set; }
        public int QuantityOfFollowing { get; set; }
        public string ProfileDescription { get; set; } = string.Empty;





    }
}
