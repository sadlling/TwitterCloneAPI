﻿using TwitterCloneAPI.Models.HashtagResponse;
using TwitterCloneAPI.Models.ServiceResponse;

namespace TwitterCloneAPI.Services.Hashtags
{
    public interface IHashtagService
    {
        public Task<ResponseModel<int>> AddHashtags(string[] hashtags, int tweetId);
        public Task<ResponseModel<List<HashtagResponseModel>>> GetHashtags();
    }
}
