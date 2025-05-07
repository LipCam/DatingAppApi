using DatingAppApi.DAL.Entities;
using System.Linq.Expressions;

namespace DatingAppApi.DAL.Repositories.Interfaces
{
    public interface IUsersRepository : IRepositoryBase<AppUsers>
    {
        Task<bool> UserExistsAsync(string userName);
        Task<AppUsers> GetUserByUserNameAsync(string userName);
        IQueryable<AppUsers> GetAllUserAsync(Expression<Func<AppUsers, bool>> filter = null);
    }
}
