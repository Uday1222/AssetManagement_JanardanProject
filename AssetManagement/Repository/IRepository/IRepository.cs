using System.Linq.Expressions;

namespace AssetManagement.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task CreateAsync(T entity);

        Task RemoveAsync(T entity);

        Task UpdateEntityAsync(T entity);

        Task SaveAsync();

        Task<List<T>>? GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, int pageSize = 0, int pageNumber = 1);

        Task<T> Get(Expression<Func<T, bool>> filter = null, bool tracked = true, string? includeProperties = null);
    }
}
