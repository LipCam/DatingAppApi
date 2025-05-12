using DatingAppApi.DAL.DB;
using DatingAppApi.DAL.Entities;
using DatingAppApi.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DatingAppApi.DAL.Repositories
{
    public class MessagesRepository : IMessagesRepository
    {
        protected AppDBContext _dbContext;

        public MessagesRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddMessage(Messages message)
        {
            _dbContext.Add(message);
        }

        public void DeleteMessage(Messages message)
        {
            _dbContext.Remove(message);
        }

        public IQueryable<Messages> GetAll(Expression<Func<Messages, bool>>? filter)
        {
            return _dbContext.Messages.Where(filter);
        }

        public async Task<Messages?> GetMessage(long id)
        {
            return await _dbContext.Messages.FindAsync(id);
        }

        public IQueryable<Messages> GetMessageThread(string currentUsername, string recipientUsername)
        {
            return _dbContext.Messages
                .Include(p=> p.Sender).ThenInclude(p=> p.Photos)
                .Include(p => p.Recipient).ThenInclude(p => p.Photos)
                .Where(p=> p.RecipientUsername == currentUsername 
                            && p.RecipientDeleted == false 
                            && p.SenderUsername == recipientUsername || 
                        p.SenderUsername == currentUsername 
                            && p.SenderDeleted == false 
                            && p.RecipientUsername == recipientUsername)
                .OrderBy(p=> p.MessageSent);
        }

        public IQueryable<Messages> GetUnreadMessages(string currentUsername)
        {
            return _dbContext.Messages.Where(p => p.RecipientUsername == currentUsername && p.DateRead == null);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
