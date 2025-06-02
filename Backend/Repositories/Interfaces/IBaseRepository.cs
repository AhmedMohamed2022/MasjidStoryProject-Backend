using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{

    public interface IBaseRepository<T> where T : class
    {
        //Task<T> GetByIdAsync(object id);
        //Task<IEnumerable<T>> GetAllAsync();
        // get  by Id of any type
        Task<T> GetByIdAsync<TKey>(TKey id);
        // Get all entities with optional includes and filters
        Task<List<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes);
        Task<List<T>> GetListWithIncludePagedAsync(
    Expression<Func<T, bool>> filter,
    int pageNumber,
    int pageSize,
    params Expression<Func<T, object>>[] includes);

        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate = null);
        Task SaveChangesAsync();

    }

}
