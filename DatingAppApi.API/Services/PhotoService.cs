using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingAppApi.API.Helpers;
using DatingAppApi.API.Services.Interfaces;
using DatingAppApi.BLL.DTOs;
using DatingAppApi.BLL.DTOs.Users;
using DatingAppApi.DAL.Entities;
using DatingAppApi.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Options;

namespace DatingAppApi.API.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;
        private readonly IUsersRepository _usersRepository;
        private readonly IMapper _mapper;

        public PhotoService(IOptions<CloudinarySettings> config, IUsersRepository usersRepository, IMapper mapper)
        {
            var acc = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);

            _cloudinary = new Cloudinary(acc);

            _usersRepository = usersRepository;
            _mapper = mapper;
        }

        public async Task<ResultDTO<PhotosDTO>> AddPhotoAsync(string userName, IFormFile file)
        {
            AppUsers appUsers = await _usersRepository.GetUserByUserNameAsync(userName);

            if (appUsers == null)
                return ResultDTO<PhotosDTO>.Failure("Cannot update user");

            var result = await AddPhotoCloudinary(file);

            if (result.Error != null)
                return ResultDTO<PhotosDTO>.Failure(result.Error.Message);

            bool IsMain = appUsers.Photos.Count == 0 ? true : false;
            //teste

            Photos photos = new Photos()
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                IsMain = IsMain
            };

            appUsers.Photos.Add(photos);

            if (await _usersRepository.SaveChangesAsync() > 0)
                return ResultDTO<PhotosDTO>.Success(_mapper.Map<PhotosDTO>(photos));

            return ResultDTO<PhotosDTO>.Failure("Problem adding photo");
        }

        private async Task<ImageUploadResult> AddPhotoCloudinary(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
                    Folder = "da-net8"
                };

                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }

            return uploadResult;
        }

        public async Task<ResultDTO<string>> DeletePhotoAsync(string userName, int photoId)
        {
            AppUsers appUsers = await _usersRepository.GetUserByUserNameAsync(userName);

            if (appUsers == null)
                return ResultDTO<string>.Failure("Cannot find user");

            Photos? photos = appUsers.Photos.FirstOrDefault(p => p.Id == photoId);

            if (photos == null)
                return ResultDTO<string>.Failure("Cannot find the photo");

            if (photos.IsMain)
                return ResultDTO<string>.Failure("Cannot delete a main photo");

            DeletionResult deletionResult = await DeletePhotoCloudinaryAsync(photos.PublicId);

            if (deletionResult.Error != null)
                return ResultDTO<string>.Failure(deletionResult.Error.Message);

            appUsers.Photos.Remove(photos);

            if (await _usersRepository.SaveChangesAsync() > 0)
                return ResultDTO<string>.Success("");

            return ResultDTO<string>.Failure("Problem deleting photo");
        }

        private async Task<DeletionResult> DeletePhotoCloudinaryAsync(string publicId)
        {
            var deleteParam = new DeletionParams(publicId);
            return await _cloudinary.DestroyAsync(deleteParam);
        }
    }
}
