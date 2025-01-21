using DatingAppApi.BLL.DTOs;
using DatingAppApi.BLL.DTOs.Users;

namespace DatingAppApi.BLL.Services.Interfaces
{
    public interface IAccountService
    {
        Task<ResultDTO<UsersRespDTO>> Register(RegisterDTO registerDTO);
        Task<ResultDTO<UsersRespDTO>> Login(LoginDTO loginDTO);
    }
}
