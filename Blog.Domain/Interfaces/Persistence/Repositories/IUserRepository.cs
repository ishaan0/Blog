using Blog.Domain.IdentityEntities;
using Blog.Domain.Models;

namespace Blog.Domain.Interfaces.Persistence.Repositories;

public interface IUserRepository
{
    Task<PaginatedList<User>> GetAllAsync(
        PaginationQuery<User> query, CancellationToken cancellationToken = default);
    Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task CreateAsync(User user, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateAsync(User user, CancellationToken cancellationToken = default);
}

