using DatingAppApi.API.Extensions;
using DatingAppApi.BLL.DTOs;
using DatingAppApi.BLL.DTOs.Messages;
using DatingAppApi.BLL.Helpers;
using DatingAppApi.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingAppApi.API.Controllers
{
    [Authorize]
    public class MessagesController : BaseApiController
    {
        IMessagesService _service;

        public MessagesController(IMessagesService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult> CreateMessage(CreateMessageDTO createMessageDTO)
        {
            var userName = User.GetUserName();
            if(userName.ToLower() == createMessageDTO.RecipientUsername.ToLower())
                return BadRequest("You cannot message yourself");

            ResultDTO<MessagesDTO> resultDTO = await _service.CreateMessage(userName, createMessageDTO);
            if (!resultDTO.IsSuccess)
                return BadRequest(resultDTO.Error);

            return Ok(resultDTO.Value);
        }

        [HttpGet]
        public async Task<ActionResult> GetMessagesForUser([FromQuery] MessageParams messageParams) 
        {
            messageParams.Username = User.GetUserName();

            var messages = await _service.GetMessagesForUser(messageParams);

            Response.AddPaginationHeader(messages);

            return Ok(messages);
        }

        [HttpGet("thread/{username}")]
        public async Task<ActionResult> GetMessageThread(string username)
        {
            var currentUsername = User.GetUserName();

            return Ok(await _service.GetMessageThread(currentUsername, username));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(long id)
        {
            var username = User.GetUserName();

            ResultDTO<string> result = await _service.DeleteMessage(id, username);

            if (result.IsSuccess)
                return Ok();

            if (!string.IsNullOrEmpty(result.Error))
                return BadRequest(result.Error);

            return Forbid();
        }
    }
}
