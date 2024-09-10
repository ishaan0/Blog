using Blog.Application.Interfaces.Repositories;
using Blog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Blog.Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public IQueryable<T> GetAll(
        bool trackChanges,
        CancellationToken cancellationToken = default) => !trackChanges ? _dbSet.AsNoTracking() : _dbSet;

    public IQueryable<T> GetByCondition(
        Expression<Func<T, bool>> expression,
        bool trackChanges,
        CancellationToken cancellationToken = default) => !trackChanges
        ? _dbSet.AsNoTracking().Where(expression) : _dbSet.Where(expression);

    public void Create(T entity, CancellationToken cancellationToken = default) => _dbSet.Add(entity);

    public void Update(T entity, CancellationToken cancellationToken = default) => _dbSet.Update(entity);

    public void Delete(T entity, CancellationToken cancellationToken = default) => _dbSet.Remove(entity);

    public async Task SaveAsync(
        CancellationToken cancellationToken = default) => await _context.SaveChangesAsync(cancellationToken);
}
