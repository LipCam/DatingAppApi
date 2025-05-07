using DatingAppApi.BLL.DTOs;
using DatingAppApi.BLL.DTOs.Messages;
using DatingAppApi.BLL.Helpers;

namespace DatingAppApi.BLL.Services.Interfaces
{
    public interface IMessagesService
    {
        Task<ResultDTO<MessagesDTO>> CreateMessage(string userName, CreateMessageDTO createMessageDTO);
        Task<PagedList<MessagesDTO>> GetMessagesForUser(MessageParams messageParams);
        Task<IEnumerable<MessagesDTO>> GetMessageThread(string currentUsername, string recipientUsername);
        Task<ResultDTO<string>> DeleteMessage(long id, string username);
    }
}
