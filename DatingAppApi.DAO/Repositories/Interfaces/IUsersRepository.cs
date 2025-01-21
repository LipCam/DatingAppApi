using DatingAppApi.DAL.Entities;

namespace DatingAppApi.DAL.Repositories.Interfaces
{
    public interface IUsersRepository : IRepositoryBase<AppUsers>
    {
        Task<bool> UserExistsAsync(string userName);
        Task<AppUsers> GetUserByUserNameAsync(string userName);
        Task<List<AppUsers>> GetAllUserAsync();
    }
}
