using DatingAppApi.BLL.DTOs;
using DatingAppApi.BLL.DTOs.Users;
using DatingAppApi.BLL.Helpers;
using DatingAppApi.DAL.Entities;
using System.Linq.Expressions;

namespace DatingAppApi.BLL.Services.Interfaces
{
    public interface IUsersService
    {
        Task<List<AppUsers>> GetAll(Expression<Func<AppUsers, bool>>? filter = null);
        Task<AppUsers> FirstOrDefault(Expression<Func<AppUsers, bool>>? filter = null);
        Task<AppUsers> Find(params object[] parametros);
        Task<AppUsers> Add(AppUsers entity);
        Task Update(AppUsers entity);
        void Delete(AppUsers entity);

        Task<MemberDTO?> GetUserByUserNameAsync(string userName);
        //Task<List<MemberDTO>> GetAllUserAsync();
        Task<PagedList<MemberDTO>> GetAllUserAsync(UserParams userParams);
        Task<string> UpdateUser(string userName, MemberUpdateDTO memberUpdateDTO);

        Task<string> SetMainPhoto(string userName, int photoId);

        Task<List<AppUserWithRolesDTO>> GetUsersWithRole();
        Task<ResultDTO<IList<string>>> EditRoles(string username, string roles);
    }
}
