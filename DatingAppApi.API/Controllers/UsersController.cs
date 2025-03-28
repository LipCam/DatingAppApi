using DatingAppApi.API.Extensions;
using DatingAppApi.API.Services.Interfaces;
using DatingAppApi.BLL.DTOs;
using DatingAppApi.BLL.DTOs.Users;
using DatingAppApi.BLL.Helpers;
using DatingAppApi.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingAppApi.API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUsersService _service;
        private readonly IPhotoService _photoService;

        public UsersController(IUsersService service, IPhotoService photoService)
        {
            _service = service;
            _photoService = photoService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery]UserParams userParams)
        {
            userParams.CurrentUserName = User.GetUserName();

            var users = await _service.GetAllUserAsync(userParams);

            Response.AddPaginationHeader(users);

            return Ok(users);
        }
        
        //[HttpGet("{Id}")]
        //public async Task<ActionResult> GetUser(long Id)
        //{
        //    return Ok(await _service.Find(Id));
        //}

        [HttpGet("{username}")]
        public async Task<ActionResult> GetUser(string userName = "")
        {
            var user = await _service.GetUserByUserNameAsync(userName);

            if (user == null) 
                return NotFound();

            return Ok(user);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(MemberUpdateDTO memberUpdateDTO)
        {
            var userName = User.GetUserName();

            string Result = await _service.UpdateUser(userName, memberUpdateDTO);

            if (Result != "")
                return BadRequest(Result);

            return NoContent();
        }

        [HttpPost("add-photo")]
        public async Task<IActionResult> AddPhoto(IFormFile file)
        {
            var userName = User.GetUserName();

            ResultDTO<PhotosDTO> result = await _photoService.AddPhotoAsync(userName, file);

            if (result.IsSuccess)
                return CreatedAtAction(nameof(GetUser), new { username = userName }, result.Value);

            return BadRequest(result.Error);
        }

        [HttpPut("set-main-photo/{photoId:int}")]
        public async Task<IActionResult> SetMainPhoto(int photoId)
        {
            string Result = await _service.SetMainPhoto(User.GetUserName(), photoId);

            if (Result == "")
                return NoContent();

            return BadRequest(Result);
        }

        [HttpDelete("delete-photo/{photoId:int}")]
        public async Task<IActionResult> DeletePhoto(int photoId)
        {
            var userName = User.GetUserName();

            ResultDTO<string> result = await _photoService.DeletePhotoAsync(userName, photoId);

            if (result.IsSuccess)
                return Ok(result.Value);

            return BadRequest(result.Error);
        }
    }
}
