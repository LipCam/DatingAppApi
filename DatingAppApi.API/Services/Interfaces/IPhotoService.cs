using DatingAppApi.BLL.DTOs;
using DatingAppApi.BLL.DTOs.Users;

namespace DatingAppApi.API.Services.Interfaces
{
    public interface IPhotoService
    {
        Task<ResultDTO<PhotosDTO>> AddPhotoAsync(string userName, IFormFile file);
        Task<ResultDTO<string>> DeletePhotoAsync(string userName, int photoId);
    }
}
