using DatingAppApi.DAL.Entities;

namespace DatingAppApi.DAL.Repositories.Interfaces
{
    public interface IUserLikesRepository
    {
        Task<UserLikes?> GetUserLike(long sourceUserId, long targetUserId);

        //Task<IEnumerable<AppUsers>> GetUserLikes(string predicate, long userId);
        IQueryable<AppUsers> GetUserLikes(string predicate, long userId);

        //Task<IEnumerable<long>> GetCurrentUserLikeIds(long currentUserId);
        IQueryable<long> GetCurrentUserLikeIds(long currentUserId);

        void DeleteLike(UserLikes like);

        void AddLike(UserLikes like);

        Task<bool> SaveChanges();
    }
}
