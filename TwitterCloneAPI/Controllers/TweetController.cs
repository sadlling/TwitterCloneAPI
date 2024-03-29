﻿using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using TwitterCloneAPI.Models;
using TwitterCloneAPI.Models.ServiceResponse;
using TwitterCloneAPI.Models.TweetRequest;
using TwitterCloneAPI.Models.TweetResponse;
using TwitterCloneAPI.Paging;
using TwitterCloneAPI.Services.Tweets;

namespace TwitterCloneAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TweetController : ControllerBase
    {
        private readonly ITweetService _tweetService;
        public TweetController(ITweetService tweetService)
        {
            _tweetService = tweetService;
        }

        [HttpPost("CreateTweet")]
        public async Task<IActionResult> CreateTweet([FromForm] TweetRequestModel request)
        {
            if (Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)) <= 0)
            {
                return Unauthorized();
            }
            var response = await _tweetService.CreateTweet(request, Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            string hostUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/";
            if (response.Data is not null)
            {
                if (!string.IsNullOrEmpty(response.Data.Image))
                {
                    response.Data.Image = $"{hostUrl}{response.Data.Image}";
                }
                if (!string.IsNullOrEmpty(response.Data.PostedUserImage))
                {
                    response.Data.PostedUserImage = $"{hostUrl}{response.Data.PostedUserImage}";
                }
                return Ok(response);
            }
            return BadRequest(response);

        }

        [HttpGet("GetAllTweets")]
        public async Task<IActionResult> GetAllTweets([FromHeader] string[] hashtags, [FromQuery] QueryStringParameters parameters)
        {
            var responce = new ResponseModel<List<TweetResponseModel>>();
            if (hashtags.Length <= 0)
            {
                responce = await _tweetService.GetAllTweets(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            }
            else
            {
                responce = await _tweetService.GetTweetsByHashtags(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)), hashtags);
            }
            string hostUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/";
            if (responce.Data is not null)
            {
                responce.Data.ForEach(x =>
                {
                    if (!string.IsNullOrEmpty(x.Image))
                    {
                        x.Image = $"{hostUrl}{x.Image}";
                    }
                    if (!string.IsNullOrEmpty(x.PostedUserImage))
                    {
                        x.PostedUserImage = $"{hostUrl}{x.PostedUserImage}";
                    }
                });
                var pagedResult = PagedList<TweetResponseModel>.ToPagedList(responce.Data, parameters.PageNumber, parameters.PageSize);
                var metadata = new
                {
                    pagedResult.TotalCount,
                    pagedResult.PageSize,
                    pagedResult.CurrentPage,
                    pagedResult.TotalPages,
                    pagedResult.HasNext,
                    pagedResult.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(pagedResult);
            }
            return BadRequest(responce);

        }
        [HttpGet("GetTweetsByHastags")]
        public async Task<IActionResult> GetTweetsByHastags([FromQuery(Name = "Hastags")] string[] hashtags)
        {
            var responce = await _tweetService.GetTweetsByHashtags(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)), hashtags);
            string hostUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/";
            if (responce.Data is not null)
            {
                responce.Data.ForEach(x =>
                {
                    if (!string.IsNullOrEmpty(x.Image))
                    {
                        x.Image = $"{hostUrl}{x.Image}";
                    }
                    if (!string.IsNullOrEmpty(x.PostedUserImage))
                    {
                        x.PostedUserImage = $"{hostUrl}{x.PostedUserImage}";
                    }
                });

                return Ok(responce);
            }
            return BadRequest(responce);

        }
        [HttpGet("GetUserTweetsAndRetweets{userId}")]
        public async Task<IActionResult> GetUserTweetsAndRetweets(int userId, [FromQuery] QueryStringParameters parameters)
        {
            if (Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)) <= 0)
            {
                return Unauthorized();
            }
            var responce = await _tweetService.GetCurrentUserTweetsAndRetweets(userId, Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            string hostUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/";
            if (responce.Data is not null)
            {
                responce.Data.ForEach(x =>
                {
                    if (!string.IsNullOrEmpty(x.Image))
                    {
                        x.Image = $"{hostUrl}{x.Image}";
                    }
                    if (!string.IsNullOrEmpty(x.PostedUserImage))
                    {
                        x.PostedUserImage = $"{hostUrl}{x.PostedUserImage}";
                    }
                });
                var pagedResult = PagedList<TweetResponseModel>.ToPagedList(responce.Data, parameters.PageNumber, parameters.PageSize);
                var metadata = new
                {
                    pagedResult.TotalCount,
                    pagedResult.PageSize,
                    pagedResult.CurrentPage,
                    pagedResult.TotalPages,
                    pagedResult.HasNext,
                    pagedResult.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(pagedResult);
            }
            if (responce.Data is null && responce.Success)
            {
                return Ok(responce);
            }
            return BadRequest(responce);

        }



        [HttpPut("UpdateTweet{tweetId}")]
        public async Task<IActionResult> UpdateTweet([FromForm] UpdateTweetRequestModel request, int tweetId)
        {
            if (Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)) <= 0)
            {
                return Unauthorized();
            }
            var response = await _tweetService.UpdateTweet(request, Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)), tweetId);
            string hostUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/";
            if (response.Data is not null)
            {
                if (!string.IsNullOrEmpty(response.Data.Image))
                {
                    response.Data.Image = $"{hostUrl}{response.Data.Image}";
                }
                if (!string.IsNullOrEmpty(response.Data.PostedUserImage))
                {
                    response.Data.PostedUserImage = $"{hostUrl}{response.Data.PostedUserImage}";
                }
                return Ok(response);
            }
            return BadRequest(response);

        }
        [HttpDelete("DeleteTweetById{tweetId}")]
        public async Task<IActionResult> DeleteTweet(int tweetId)
        {
            if (Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)) <= 0)
            {
                return Unauthorized();
            }
            var responce = await _tweetService.DeleteTweet(tweetId);
            if (responce.Success)
            {
                return Ok(responce);
            }
            return BadRequest(responce);
        }


    }
}
