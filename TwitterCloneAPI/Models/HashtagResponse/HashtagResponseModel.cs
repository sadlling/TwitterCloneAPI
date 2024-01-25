namespace TwitterCloneAPI.Models.HashtagResponse
{
    public class HashtagResponseModel
    {
        public int HashtagId { get; set; }
        public string HashtagName { get; set; } = null!;
        public int TweetsCount { get; set; }

    }
}
