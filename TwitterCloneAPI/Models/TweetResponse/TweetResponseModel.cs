﻿using Microsoft.AspNetCore.Http.HttpResults;
using TwitterCloneAPI.Models;

namespace TwitterCloneAPI.Models.TweetResponse
{
    public class TweetResponseModel
    {
        public int TweetId {  get; set; }
        public int PostedUserId { get; set; }
        public string PostedUserName {  get; set; } = string.Empty!;
        public string PostedUserImage { get; set; } = string.Empty!;
        public string Content { get; set; } = string.Empty!;
        public string Image { get; set; } = string.Empty!;
        public bool IsPublic { get; set; } = true;
        public DateTime CreatedAt {  get; set; }
        public int CommentsCount { get; set; }
        public int RetweetCount { get; set; }
        public int LikesCount { get; set; }
        public int SaveCount {  get; set; }
        public bool IsRetweeted {  get; set; } = false;
        public bool IsLiked { get; set; } = false;
        public bool IsSaved { get; set; } = false;
        public bool IsOwner {  get; set; } = false;

    }
}
