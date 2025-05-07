using DatingAppApi.DAL.DB;
using DatingAppApi.DAL.Entities;
using DatingAppApi.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingAppApi.DAL.Repositories
{
    public class UserLikesRepository : IUserLikesRepository
    {
        protected AppDBContext _dbContext;

        public UserLikesRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddLike(UserLikes like)
        {
            _dbContext.UserLikes.Add(like);
        }

        public void DeleteLike(UserLikes like)
        {
            _dbContext.Remove(like);
        }

        public IQueryable<long> GetCurrentUserLikeIds(long currentUserId)
        {
            return _dbContext.UserLikes
                .Where(p => p.SourceUserId == currentUserId)
                .Select(p => p.TargetUserId);
        }

        public async Task<UserLikes?> GetUserLike(long sourceUserId, long targetUserId)
        {
            return await _dbContext.UserLikes.FindAsync(sourceUserId, targetUserId);
        }

        public IQueryable<AppUsers> GetUserLikes(string predicate, long userId)
        {
            var likes = _dbContext.UserLikes.AsQueryable();

            switch (predicate)
            {
                case "liked":
                    return likes
                        .Where(p => p.SourceUserId == userId)
                        .Select(p => p.TargetUser);
                case "likedBy":
                    return  likes
                        .Where(p => p.TargetUserId == userId)
                        .Select(p => p.SourceUser);
                default:
                    var likeIds =  GetCurrentUserLikeIds(userId);

                    return  likes
                        .Where(p => p.TargetUserId == userId && likeIds.Contains(p.SourceUserId))
                        .Select(p => p.SourceUser);
            }
        }

        //public async Task<IEnumerable<AppUsers>> GetUserLikes(string predicate, long userId)
        //{
        //    var likes = _dbContext.UserLikes.AsQueryable();

        //    switch (predicate)
        //    {
        //        case "liked":
        //            return await likes
        //                .Where(p=> p.SourceUserId == userId)
        //                .Include(p=> p.TargetUser.Photos)
        //                .Select(p=> p.TargetUser)
        //                .ToListAsync();
        //        case "likedBy":
        //            return await likes
        //                .Where(p => p.TargetUserId == userId)
        //                .Include(p => p.SourceUser.Photos)
        //                .Select(p => p.SourceUser)
        //                .ToListAsync();
        //        default:
        //            var likeIds = await GetCurrentUserLikeIds(userId);

        //            return await likes
        //                .Where(p => p.TargetUserId == userId && likeIds.Contains(p.SourceUserId))
        //                .Include(p => p.SourceUser.Photos)
        //                .Select(p => p.SourceUser)
        //                .ToListAsync();
        //    }
        //}

        public async Task<bool> SaveChanges()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
