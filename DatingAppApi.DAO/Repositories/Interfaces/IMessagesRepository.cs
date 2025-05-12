using DatingAppApi.DAL.Entities;
using System.Linq.Expressions;

namespace DatingAppApi.DAL.Repositories.Interfaces
{
    public interface IMessagesRepository
    {
        void AddMessage(Messages message);
        void DeleteMessage(Messages message);
        Task<Messages?> GetMessage(long id);
        IQueryable<Messages> GetAll(Expression<Func<Messages, bool>>? filter = null);
        IQueryable<Messages> GetMessageThread(string currentUsername, string recipientUsername);
        IQueryable<Messages> GetUnreadMessages(string currentUsername);
        Task<bool> SaveAllAsync();
    }
}
