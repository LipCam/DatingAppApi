using DatingAppApi.DAL.DB;
using DatingAppApi.DAL.Entities;
using DatingAppApi.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DatingAppApi.DAL.Repositories
{
    public class UsersRepository : RepositoryBase<AppUsers>, IUsersRepository
    {
        public UsersRepository(AppDBContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<AppUsers> GetAllUserAsync(Expression<Func<AppUsers, bool>> filter = null)
        {
            return _dbContext.AppUsers.Where(filter);
        }

        public async Task<AppUsers?> GetUserByUserNameAsync(string userName)
        {
            return await _dbContext.AppUsers
                .Include(x => x.Photos)
                .SingleOrDefaultAsync(p => p.UserName == userName);
        }

        public async Task<bool> UserExistsAsync(string userName)
        {
            return await _dbContext.AppUsers.AnyAsync(x => x.UserName.ToLower() == userName.ToLower());
        }
    }
}
