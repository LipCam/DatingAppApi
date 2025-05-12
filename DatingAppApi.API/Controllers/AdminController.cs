using DatingAppApi.BLL.DTOs;
using DatingAppApi.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingAppApi.API.Controllers
{
    public class AdminController : BaseApiController
    {
        IUsersService _service;

        public AdminController(IUsersService service)
        {
            _service = service;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRole()
        {
            return Ok(await _service.GetUsersWithRole());
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditRoles(string username, string roles)
        {
            if (string.IsNullOrEmpty(roles))
                return BadRequest("You must select at least one role");

            ResultDTO<IList<string>> resultDTO = await _service.EditRoles(username, roles);

            if(!resultDTO.IsSuccess)
                return BadRequest(resultDTO.Error);

            return Ok(resultDTO.Value);
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("photos-to-moderate")]
        public async Task<ActionResult> GetPhotosForModeration()
        {

            return Ok("photos-to-moderate");
        }
    }
}
