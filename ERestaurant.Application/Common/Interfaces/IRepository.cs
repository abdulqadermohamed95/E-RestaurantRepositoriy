using ERestaurant.Application.Common.Pageination;
using System.Linq.Expressions;

namespace ERestaurant.Application.Common.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        IQueryable<T> Query();  
        Task<PagedResult<T>> GetAllAsync(
             Expression<Func<T, bool>>? filter = null,
             Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
             int pageNumber = 1,
             int pageSize = 10);
    }
}
