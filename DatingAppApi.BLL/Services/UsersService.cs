using AutoMapper;
using DatingAppApi.BLL.DTOs.Users;
using DatingAppApi.BLL.Helpers;
using DatingAppApi.BLL.Services.Interfaces;
using DatingAppApi.DAL.Entities;
using DatingAppApi.DAL.Repositories.Interfaces;
using System.Linq.Expressions;

namespace DatingAppApi.BLL.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _repository;
        private readonly IMapper _mapper;

        public UsersService(IUsersRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<AppUsers>> GetAll(Expression<Func<AppUsers, bool>> filter = null)
        {
            return await _repository.GetAllAsync(filter);
        }

        public async Task<AppUsers> FirstOrDefault(Expression<Func<AppUsers, bool>> filter = null)
        {
            return await _repository.FirstOrDefaultAsync(filter);
        }

        public async Task<AppUsers> Find(params object[] parametros)
        {
            return await _repository.FindAsync(parametros);
        }

        public async Task<AppUsers> Add(AppUsers entity)
        {
            return await _repository.AddAsync(entity);
        }

        public async Task Update(AppUsers entity)
        {
            await _repository.Update(entity);
        }

        public void Delete(AppUsers entity)
        {
            _repository?.Delete(entity);
        }

        public async Task<MemberDTO?> GetUserByUserNameAsync(string userName)
        {
            var user = await _repository.GetUserByUserNameAsync(userName);
            return _mapper.Map<MemberDTO>(user);
        }

        //public async Task<List<MemberDTO>> GetAllUserAsync()
        //{
        //    var users = await _repository.GetAllUserAsync();            
        //    return _mapper.Map<List<MemberDTO>>(users);
        //}
        public async Task<PagedList<MemberDTO>> GetAllUserAsync(UserParams userParams)
        {
            var mimDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge - 1));
            var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));

            var users = await _repository.GetAllUserAsync(p=> p.UserName != userParams.CurrentUserName 
                                                                && (string.IsNullOrEmpty(userParams.Gender) || p.Gender == userParams.Gender)
                                                                && p.DateOfBirth >= mimDob && p.DateOfBirth <= maxDob);

            switch (userParams.Orderby)
            {
                case "created":
                    users = users.OrderByDescending(p => p.Created).ToList();
                    break;
                default:
                    users = users.OrderByDescending(p => p.LastActive).ToList();
                    break;
            }            

            var lst = _mapper.Map<List<MemberDTO>>(users);

            return PagedList<MemberDTO>.Create(lst, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<string> UpdateUser(string userName, MemberUpdateDTO memberUpdateDTO)
        {
            string Message = "";

            AppUsers appUsers = await _repository.GetUserByUserNameAsync(userName);

            if (appUsers != null)
            {
                _mapper.Map(memberUpdateDTO, appUsers);
                await _repository.Update(appUsers);
            }
            else
                Message = "Could not find user";

            return Message;
        }

        public async Task<string> SetMainPhoto(string userName, int photoId)
        {
            string Message = "";

            var user = await _repository.GetUserByUserNameAsync(userName);

            if (user != null)
            {
                var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);

                if (photo != null)
                {
                    var currentMain = user.Photos.FirstOrDefault(p => p.IsMain);
                    if (currentMain != null)
                        currentMain.IsMain = false;

                    photo.IsMain = true;

                    await _repository.SaveChangesAsync();
                }
                else
                    Message = "Could not find photo";
            }
            else
                Message = "Could not find user";

            return Message;
        }
    }
}
