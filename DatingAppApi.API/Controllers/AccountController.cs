using DatingAppApi.BLL.DTOs;
using DatingAppApi.BLL.DTOs.Users;
using DatingAppApi.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DatingAppApi.API.Controllers
{
    public class AccountController : BaseApiController
    {
        IAccountService _service;

        public AccountController(IAccountService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDTO registerDTO)
        {
            ResultDTO<UsersRespDTO> resultDTO = await _service.Register(registerDTO);
            if (!resultDTO.IsSuccess)
                return BadRequest(resultDTO.Error);

            return Ok(resultDTO.Value);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDTO loginDTO)
        {
            ResultDTO<UsersRespDTO> resultDTO = await _service.Login(loginDTO);
            if (!resultDTO.IsSuccess)
                return BadRequest(resultDTO.Error);

            return Ok(resultDTO.Value);
        }
    }
}
