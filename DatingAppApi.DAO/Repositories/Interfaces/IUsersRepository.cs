using DatingAppApi.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;

namespace DatingAppApi.DAL.Repositories.Interfaces
{
    public interface IUsersRepository : IRepositoryBase<AppUsers>
    {
        Task<bool> UserExistsAsync(string userName);
        Task<AppUsers> GetUserByUserNameAsync(string userName);
        IQueryable<AppUsers> GetAllUser(Expression<Func<AppUsers, bool>>? filter = null);
        Task<IdentityResult> CreateAsync(AppUsers appUser, string password);
        Task<bool> CheckPasswordAsync(AppUsers appUser, string password);
        Task<IList<string>> GetRolesAsync(AppUsers appUser);
        Task<IdentityResult> AddToRolesAsync(AppUsers appUser, IEnumerable<string> roles);
        Task<IdentityResult> RemoveFromRolesAsync(AppUsers appUser, IEnumerable<string> roles);
    }
}
