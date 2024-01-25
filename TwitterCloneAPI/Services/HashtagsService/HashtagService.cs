﻿using Azure.Core;
using Microsoft.EntityFrameworkCore;
using TwitterCloneAPI.Models;
using TwitterCloneAPI.Models.HashtagResponse;
using TwitterCloneAPI.Models.ServiceResponse;

namespace TwitterCloneAPI.Services.HashtagsService
{
    public class HashtagService : IHashtagService
    {
        private readonly TwitterCloneContext _context;
        public HashtagService(TwitterCloneContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<int>> AddHashtags(string[] hashtags, int tweetId)
        {
            var response = new ResponseModel<int>();
            try
            {
                if (hashtags.Length > 0)
                {
                    var existingHashtags = _context.Hashtags.Where(x =>hashtags.Select(y => y).Contains(x.Name)).ToList();
                    var newHashtags = hashtags.Except(existingHashtags.Select(x => x.Name)).ToList();
                    if (newHashtags.Count > 0)
                    {
                        var hashtagsToAdd = newHashtags.Select(x => new Hashtag { Name = x, CreatedAt = DateTime.Now }).ToList();
                        foreach (var item in hashtagsToAdd)
                        {
                            await _context.Hashtags.AddAsync(item);
                        }
                        await _context.SaveChangesAsync();
                        existingHashtags.AddRange(hashtagsToAdd);
                    }
                    await _context.TweetHashtags.AddRangeAsync(existingHashtags.Select(x => new TweetHashtag { HashtagId = x.HashtagId, TweetId = tweetId }));
                    await _context.SaveChangesAsync();
                }
                response.Message = "Hashtags added in service";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }
            return response;

        }

        public async Task<ResponseModel<List<HashtagResponseModel>>> GetHashtags()
        {
            var response  = new ResponseModel<List<HashtagResponseModel>>();
            try
            {
                var hashtags = await _context.TweetHashtags.Include(x=>x.Hashtag).ToListAsync();
                response.Data = hashtags.Select(x => new HashtagResponseModel
                {
                    HashtagId=x.HashtagId,
                    HashtagName =x.Hashtag.Name,
                    TweetsCount = hashtags.Where(y=>y.HashtagId == x.HashtagId).Count(),

                }).GroupBy(p=>p.HashtagName).Select(g=>g.First()).OrderByDescending(x=>x.TweetsCount).Take(7).ToList();

                response.Success = true;
                response.Message = "Top Hastags";
            }
            catch (Exception ex)
            {
                response.Success= false;
                response.Message= ex.Message;
            }
            return response;
        }
    }
}