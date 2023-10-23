﻿namespace TwitterCloneAPI.Models.RefreshToken
{
    public class RefreshToken
    {
        public string Token { get; set; } = null!;
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Expired { get; set; } = DateTime.Now;
    }
}
