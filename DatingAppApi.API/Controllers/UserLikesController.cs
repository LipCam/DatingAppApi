using DatingAppApi.API.Extensions;
using DatingAppApi.BLL.DTOs;
using DatingAppApi.BLL.Helpers;
using DatingAppApi.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingAppApi.API.Controllers
{
    [Authorize]
    public class UserLikesController : BaseApiController
    {
        private readonly IUserLikesService _service;

        public UserLikesController(IUserLikesService service)
        {
            _service = service;
        }
                
        [HttpPost("{targetUserId:long}")]
        public async Task<IActionResult> ToogleLike(long targetUserId)
        {
            var userId = User.GetUserId();

            ResultDTO<string> result = await _service.ToogleLike(userId, targetUserId);

            if (result.IsSuccess)
                return Ok();

            return BadRequest(result.Error);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetCurrentUserLikeIds()
        {
            return Ok(await _service.GetCurrentUserLikeIds(User.GetUserId()));
        }

        [HttpGet]
        public async Task<IActionResult> GetUserLikes([FromQuery]UserLikesParams userLikesParams)
        {
            userLikesParams.UserId = User.GetUserId();

            var userLikes = await _service.GetUserLikes(userLikesParams);

            Response.AddPaginationHeader(userLikes);

            return Ok(userLikes);
        }
    }
}
