using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using DatingAppApi.DAL.DB;
using DatingAppApi.DAL.Repositories.Interfaces;

namespace DatingAppApi.DAL.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected AppDBContext _dbContext;

        public RepositoryBase(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync();
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>>? filter = null)
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(filter);
        }

        public async Task<T> FindAsync(params object[] keyValues)
        {
            return await _dbContext.Set<T>().FindAsync(keyValues);
        }

        public async Task<T> AddAsync(T entity)
        {
            if (entity != null)
            {
                _dbContext.Set<T>().Add(entity);
                await _dbContext.SaveChangesAsync();
            }

            return entity;
        }

        public async Task Update(T entity)
        {
            if (entity != null)
            {
                _dbContext.Entry(entity).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async void Delete(T entity)
        {
            if (entity != null)
            {
                _dbContext.Set<T>().Remove(entity);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
