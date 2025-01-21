using DatingAppApi.BLL.DTOs;
using DatingAppApi.DAL.Entities;

namespace DatingAppApi.BLL.Services.Interfaces
{
    public interface ITokenService
    {
        ResultDTO<string> CreateToken(AppUsers appUsers);
    }
}
