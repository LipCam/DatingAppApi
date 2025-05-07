using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingAppApi.BLL.DTOs;
using DatingAppApi.BLL.DTOs.Users;
using DatingAppApi.BLL.Helpers;
using DatingAppApi.BLL.Services.Interfaces;
using DatingAppApi.DAL.Entities;
using DatingAppApi.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingAppApi.BLL.Services
{
    public class UserLikesService : IUserLikesService
    {
        private readonly IUserLikesRepository _repository;
        private readonly IUsersRepository _userRepository;
        private readonly IMapper _mapper;

        public UserLikesService(IUserLikesRepository repository, IUsersRepository userRepository, IMapper mapper)
        {
            _repository = repository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ResultDTO<string>> ToogleLike(long sourceUserId, long targetUserId)
        {
            if (sourceUserId == targetUserId)
                return ResultDTO<string>.Failure("You cannot like yourself");
                        
            if(await _userRepository.FindAsync(sourceUserId) == null)
                return ResultDTO<string>.Failure("Source user not found");

            if(await _userRepository.FindAsync(targetUserId) == null)
                return ResultDTO<string>.Failure("Target user not found");

            UserLikes userLikes = await _repository.GetUserLike(sourceUserId, targetUserId);

            if(userLikes == null)
            {
                UserLikes newUserLikes = new UserLikes
                {
                    SourceUserId = sourceUserId,
                    TargetUserId = targetUserId
                };

                _repository.AddLike(newUserLikes);
            }
            else
            {
                _repository.DeleteLike(userLikes);
            }

            if(await _repository.SaveChanges())
                return ResultDTO<string>.Success(string.Empty);

            return ResultDTO<string>.Failure("Failure to update like");
        }        

        public async Task<IEnumerable<long>> GetCurrentUserLikeIds(long currentUserId)
        {
            return await _repository.GetCurrentUserLikeIds(currentUserId).ToListAsync();
        }

        public async Task<UserLikes?> GetUserLike(long sourceUserId, long targetUserId)
        {
            return await _repository.GetUserLike(sourceUserId, targetUserId);
        }

        public async Task<PagedList<MemberDTO>> GetUserLikes(UserLikesParams userLikesParams)
        {
            var userLikes = _repository.GetUserLikes(userLikesParams.Predicate, userLikesParams.UserId);

            var lst = userLikes.ProjectTo<MemberDTO>(_mapper.ConfigurationProvider);

            return await PagedList<MemberDTO>.CreateAsync(lst, userLikesParams.PageNumber, userLikesParams.PageSize);
        }        
    }
}
