using System.Linq.Expressions;

namespace VideoPalace.Common.Contracts;

public interface IRepository<T> where T : IEntity
{
    Task<IReadOnlyCollection<T>> GetAllAsync();
    Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter);
    Task<T?> GetAsync(Guid id);
    Task<T?> GetAsync(Expression<Func<T, bool>> filter);
    Task CreateAsync(T entity);
    Task BulkCreateAsync(IEnumerable<T> entities);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
}