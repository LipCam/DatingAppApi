using AutoMapper;
using DatingAppApi.BLL.DTOs;
using DatingAppApi.BLL.DTOs.Users;
using DatingAppApi.BLL.Services.Interfaces;
using DatingAppApi.DAL.Entities;
using DatingAppApi.DAL.Repositories.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace DatingAppApi.BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUsersRepository _repository;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountService(IUsersRepository repository, ITokenService tokenService, IMapper mapper)
        {
            _repository = repository;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<ResultDTO<UsersRespDTO>> Register(RegisterDTO registerDTO)
        {
            using var hmac = new HMACSHA512();

            AppUsers appUser = _mapper.Map<AppUsers>(registerDTO);
            appUser.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password));
            appUser.PasswordSalt = hmac.Key;

            if (await _repository.UserExistsAsync(appUser.UserName))
            {
                return ResultDTO<UsersRespDTO>.Failure("User is taken");
            }
            else
            {
                await _repository.AddAsync(appUser);

                ResultDTO<string> resultToken = _tokenService.CreateToken(appUser);
                if (!resultToken.IsSuccess)
                    return ResultDTO<UsersRespDTO>.Failure(resultToken.Error);

                UsersRespDTO usersRespDTO = new UsersRespDTO()
                {
                    UserName = appUser.UserName!,
                    KnownAs = appUser.KnownAs,
                    Token = resultToken.Value
                };
                return ResultDTO<UsersRespDTO>.Success(usersRespDTO);
            }
        }

        public async Task<ResultDTO<UsersRespDTO>> Login(LoginDTO loginDTO)
        {
            //AppUsers appUser = await _repository.FirstOrDefaultAsync(p=> p.UserName == loginDTO.UserName.ToLower());
            AppUsers appUser = await _repository.GetUserByUserNameAsync(loginDTO.UserName.ToLower());
            if (appUser == null)
            {
                return ResultDTO<UsersRespDTO>.Failure("Invalid username");
            }
            else
            {
                using var hmac = new HMACSHA512(appUser.PasswordSalt);
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != appUser.PasswordHash[i])
                        return ResultDTO<UsersRespDTO>.Failure("Invalid password");
                }
            }

            ResultDTO<string> resultToken = _tokenService.CreateToken(appUser);
            if (!resultToken.IsSuccess)
                return ResultDTO<UsersRespDTO>.Failure(resultToken.Error);

            UsersRespDTO userRespDTO = new UsersRespDTO()
            {
                UserName = appUser.UserName!,
                KnownAs = appUser.KnownAs,
                Token = resultToken.Value,
                PhotoUrl = appUser.Photos.FirstOrDefault(p=> p.IsMain)?.Url
            };

            return ResultDTO<UsersRespDTO>.Success(userRespDTO);
        }
    }
}
