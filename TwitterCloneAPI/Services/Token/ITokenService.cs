using TwitterCloneAPI.Models;
using TwitterCloneAPI.Models.RefreshToken;
using TwitterCloneAPI.Models.UserRequest;

namespace TwitterCloneAPI.Services.Token
{
    public interface ITokenService
    {
        public string CreateJwtToken(UserAuthentication user);
        public RefreshToken CreateRefreshToken();
    }
}
