using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Repositories.Implementations
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<List<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.ToListAsync();
        }
        // Tkey is type of Id, can be int, string, Guid etc.
        public async Task<T> GetByIdAsync<TKey>(TKey id)
        {
            return await _dbSet.FirstOrDefaultAsync(e => EF.Property<TKey>(e, "Id")!.Equals(id));
        }
        // returns only the first matched entity (not a list).
        /*
         * var userWithRoles = await _userRepo.GetFirstOrDefaultAsync(
           u => u.UserName == "ahmed", 
        u => u.Roles, u => u.Notifications);
         */
        public async Task<T> GetFirstOrDefaultAsync(
        Expression<Func<T, bool>> filter,
        params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
                query = query.Include(include);

            return await query.FirstOrDefaultAsync(filter);
        }
        public async Task<List<T>> GetListWithIncludePagedAsync(
        Expression<Func<T, bool>> filter,
        int pageNumber,
        int pageSize,
        params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
                query = query.Include(include);

            return await query
                .Where(filter)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }



        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
        {
            return predicate == null
                ? await _dbSet.CountAsync()
                : await _dbSet.CountAsync(predicate);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
