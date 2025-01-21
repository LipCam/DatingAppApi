using DatingAppApi.DAL.DB;
using DatingAppApi.DAL.Entities;
using DatingAppApi.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingAppApi.DAL.Repositories
{
    public class UsersRepository : RepositoryBase<AppUsers>, IUsersRepository
    {
        public UsersRepository(AppDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<AppUsers>> GetAllUserAsync()
        {
            return await _dbContext.AppUsers
                .Include(x=>x.Photos)
                .ToListAsync();
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
