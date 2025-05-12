using DatingAppApi.BLL.DTOs;
using DatingAppApi.DAL.Entities;

namespace DatingAppApi.BLL.Services.Interfaces
{
    public interface ITokenService
    {
        Task<ResultDTO<string>> CreateToken(AppUsers appUsers);
    }
}
