using TwitterCloneAPI.Models.ServiceResponse;

namespace TwitterCloneAPI.Services.Folowers
{
    public interface IFolowerService
    {
        public Task<ResponseModel<int>> AddFollower(int userId,int followerId);
        public Task<ResponseModel<int>> RemoveFollower(int userId, int followerId);
    }
}
