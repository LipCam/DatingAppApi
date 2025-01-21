using DatingAppApi.BLL.DTOs.Users;
using DatingAppApi.DAL.Entities;
using System.Linq.Expressions;

namespace DatingAppApi.BLL.Services.Interfaces
{
    public interface IUsersService
    {
        Task<List<AppUsers>> GetAll(Expression<Func<AppUsers, bool>> filter = null);
        Task<AppUsers> FirstOrDefault(Expression<Func<AppUsers, bool>> filter = null);
        Task<AppUsers> Find(params object[] parametros);
        Task<AppUsers> Add(AppUsers entity);
        void Update(AppUsers entity);
        void Delete(AppUsers entity);

        Task<MemberDTO?> GetUserByUserNameAsync(string userName);
        Task<List<MemberDTO>> GetAllUserAsync();
        Task<string> UpdateUser(string userName, MemberUpdateDTO memberUpdateDTO);

        Task<string> SetMainPhoto(string userName, int photoId);
    }
}
