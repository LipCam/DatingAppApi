using DatingAppApi.BLL.DTOs;
using DatingAppApi.BLL.DTOs.Users;
using DatingAppApi.BLL.Helpers;
using DatingAppApi.DAL.Entities;

namespace DatingAppApi.BLL.Services.Interfaces
{
    public interface IUserLikesService
    {
        Task<ResultDTO<string>> ToogleLike(long sourceUserId, long targetUserId);

        Task<UserLikes?> GetUserLike(long sourceUserId, long targetUserId);

        Task<PagedList<MemberDTO>> GetUserLikes(UserLikesParams userLikesParams);

        Task<IEnumerable<long>> GetCurrentUserLikeIds(long currentUserId);
    }
}
