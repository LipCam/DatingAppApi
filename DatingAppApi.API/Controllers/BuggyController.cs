using DatingAppApi.BLL.Services.Interfaces;
using DatingAppApi.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingAppApi.API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly IUsersService _service;

        public BuggyController(IUsersService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetAuth()
        {
            return "secret text";
        }

        [HttpGet("not-found")]
        public async Task<ActionResult<AppUsers>> GetNotFound()
        {
            var thing = await _service.Find((long)-1);
            if (thing == null) return NotFound();
            return thing;
        }

        [HttpGet("server-error")]
        public async Task<ActionResult<AppUsers>> GetServerError()
        {
            var thing = await _service.Find((long)-1) ?? throw new Exception("A bad thing has happened");
            return thing;
        }

        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("This is not a good request");
        }
    }
}
