using DatingAppApi.DAL.DB;
using DatingAppApi.DAL.Entities;
using DatingAppApi.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DatingAppApi.DAL.Repositories
{
    public class UsersRepository : RepositoryBase<AppUsers>, IUsersRepository
    {
        private readonly UserManager<AppUsers> _userManager;
        //private readonly AppDBContext _dbContext;

        public UsersRepository(UserManager<AppUsers> userManager, AppDBContext dbContext) : base(dbContext)
        {
            _userManager = userManager;
            //_dbContext = dbContext;
        }

        public async Task<IdentityResult> CreateAsync(AppUsers appUser, string password)
        {
            return await _userManager.CreateAsync(appUser, password);
        }

        public async Task<bool> CheckPasswordAsync(AppUsers appUser, string password)
        {
            return await _userManager.CheckPasswordAsync(appUser, password);
        }

        public IQueryable<AppUsers> GetAllUser(Expression<Func<AppUsers, bool>>? filter = null)
        {
            //return _dbContext.Users.Where(filter);
            if (filter != null)
                return _userManager.Users.Where(filter);
            return _userManager.Users;
        }

        public async Task<AppUsers?> GetUserByUserNameAsync(string userName)
        {
            //return await _dbContext.Users
            //    .Include(x => x.Photos)
            //    .SingleOrDefaultAsync(p => p.UserName == userName);
            return await _userManager.Users
                .Include(x => x.Photos)
                .SingleOrDefaultAsync(p => p.NormalizedUserName == userName.ToUpper());
        }

        public async Task<bool> UserExistsAsync(string userName)
        {
            //return await _dbContext.Users.AnyAsync(x => x.NormalizedUserName == userName.ToUpper());
            return await _userManager.Users.AnyAsync(x => x.NormalizedUserName == userName.ToUpper());
        }

        public async Task<IList<string>> GetRolesAsync(AppUsers appUser)
        {
            return await _userManager.GetRolesAsync(appUser);
        }

        public async Task<IdentityResult> AddToRolesAsync(AppUsers appUser, IEnumerable<string> roles)
        {
            return await _userManager.AddToRolesAsync(appUser, roles);
        }

        public async Task<IdentityResult> RemoveFromRolesAsync(AppUsers appUser, IEnumerable<string> roles)
        {
            return await _userManager.RemoveFromRolesAsync(appUser, roles);
        }
    }
}
