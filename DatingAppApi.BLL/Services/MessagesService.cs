using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingAppApi.BLL.DTOs;
using DatingAppApi.BLL.DTOs.Messages;
using DatingAppApi.BLL.Helpers;
using DatingAppApi.BLL.Services.Interfaces;
using DatingAppApi.DAL.Entities;
using DatingAppApi.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingAppApi.BLL.Services
{
    public class MessagesService : IMessagesService
    {
        IMessagesRepository _repository;
        IUsersRepository _usersRepository;
        IMapper _mapper;

        public MessagesService(IMessagesRepository repository, IUsersRepository usersRepository, IMapper mapper)
        {
            _repository = repository;
            _usersRepository = usersRepository;
            _mapper = mapper;
        }

        public async Task<ResultDTO<MessagesDTO>> CreateMessage(string userName, CreateMessageDTO createMessageDTO)
        {
            AppUsers sender = await _usersRepository.GetUserByUserNameAsync(userName);
            AppUsers recipient = await _usersRepository.GetUserByUserNameAsync(createMessageDTO.RecipientUsername);

            if (sender == null || recipient == null)
                return ResultDTO<MessagesDTO>.Failure("Fail to create message");

            Messages messages = new Messages() { 
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName!,
                RecipientUsername = recipient.UserName!,
                Content = createMessageDTO.Content,
            };

            _repository.AddMessage(messages);

            if(await _repository.SaveAllAsync())
                return ResultDTO<MessagesDTO>.Success(_mapper.Map<MessagesDTO>(messages));

            return ResultDTO<MessagesDTO>.Failure("Fail to save message");
        }

        public async Task<PagedList<MessagesDTO>> GetMessagesForUser(MessageParams messageParams)
        {
            var query = _repository.GetAll(p => (messageParams.Container == "Inbox" && p.Recipient.UserName == messageParams.Username && p.RecipientDeleted == false)
                        || (messageParams.Container == "Outbox" && p.Sender.UserName == messageParams.Username && p.SenderDeleted == false)
                        || (messageParams.Container == "Unread" && p.Recipient.UserName == messageParams.Username && p.DateRead == null && p.RecipientDeleted == false))
                .OrderByDescending(p => p.MessageSent);

            var lst = query.ProjectTo<MessagesDTO>(_mapper.ConfigurationProvider);

            return await PagedList<MessagesDTO>.CreateAsync(lst, messageParams.PageNumber, messageParams.PageSize);
        }

        //public async Task<PagedList<MessagesDTO>> GetMessagesForUser(MessageParams messageParams)
        //{
        //    var query = _repository.GetAll2().OrderByDescending(p => p.MessageSent).AsQueryable();

        //    query = messageParams.Container switch
        //    {
        //        "Inbox" => query.Where(p => p.Recipient.UserName == messageParams.Username),
        //        "Outbox" => query.Where(p => p.Sender.UserName == messageParams.Username),
        //        _ => query.Where(p => p.Recipient.UserName == messageParams.Username && p.DateRead == null),
        //    };

        //    var lst = await query.ProjectTo<MessagesDTO>(_mapper.ConfigurationProvider).ToListAsync();

        //    return PagedList<MessagesDTO>.Create(lst, messageParams.PageNumber, messageParams.PageSize);
        //}

        public async Task<IEnumerable<MessagesDTO>> GetMessageThread(string currentUsername, string recipientUsername)
        {
            List<Messages> lstMessages = await _repository.GetMessageThread(currentUsername, recipientUsername).ToListAsync();

            List<Messages> lstUnreadMessages = _repository.GetUnreadMessages(currentUsername).ToList();

            if (lstUnreadMessages.Count != 0)
            {
                lstUnreadMessages.ForEach(p => p.DateRead = DateTime.Now);
                await _repository.SaveAllAsync();
            }

            return _mapper.Map<IEnumerable<MessagesDTO>>(lstMessages);
        }

        public async Task<ResultDTO<string>> DeleteMessage(long id, string username)
        {
            Messages? messages = await _repository.GetMessage(id);

            if(messages == null)
                return ResultDTO<string>.Failure("Cannot delete this message");

            if (messages.SenderUsername != username && messages.RecipientUsername != username)
                return ResultDTO<string>.Failure("");

            if(messages.SenderUsername == username)
                messages.SenderDeleted = true;

            if (messages.RecipientUsername == username)
                messages.RecipientDeleted = true;

            if(messages is { SenderDeleted: true, RecipientDeleted: true })
                _repository.DeleteMessage(messages);

            if (await _repository.SaveAllAsync())
                return ResultDTO<string>.Success("");

            return ResultDTO<string>.Failure("Problem deleting the message");
        }
    }
}
