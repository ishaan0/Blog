using System.Linq.Expressions;

namespace Blog.Application.Interfaces.Repositories;

public interface IGenericRepository<T> where T : class
{
    IQueryable<T> GetAll(bool trackChanges, CancellationToken cancellationToken = default);
    IQueryable<T> GetByCondition(
        Expression<Func<T, bool>> expression, bool trackChanges, CancellationToken cancellationToken = default);
    void Create(T entity, CancellationToken cancellationToken = default);
    void Update(T entity, CancellationToken cancellationToken = default);
    void Delete(T entity, CancellationToken cancellationToken = default);
    Task SaveAsync(CancellationToken cancellationToken = default);
}
