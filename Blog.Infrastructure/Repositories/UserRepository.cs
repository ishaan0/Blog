using Blog.Application.Interfaces.Repositories;
using Blog.Domain.Exceptions;
using Blog.Domain.IdentityEntities;
using Blog.Domain.Messages;
using Blog.Domain.Models;
using Blog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Repositories;

internal class UserRepository(ApplicationDbContext context) : IUserRepository
{
    public async Task CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(user);
        await context.Users.AddAsync(user);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (!await context.Users.AnyAsync(u => u.Id == id, cancellationToken))
        {
            throw new NotFoundException(UserMessages.NotFound);
        }
        var user = context.Users.FirstOrDefault(u => u.Id == id);
        context.Remove(user);
    }

    public async Task<PaginatedList<User>> GetAllAsync(PaginationQuery<User> query, CancellationToken cancellationToken = default)
    {
        var queryble = context.Users
            .Where(query.Filter)
            .OrderBy(u => u.Name);

        var users = await queryble
            .Skip(query.PageSize * (query.PageNumber - 1))
            .Take(query.PageSize)
            .AsNoTracking()
            .ToListAsync();

        return new PaginatedList<User>(
            users,
            new PaginationMetadata(
                await queryble.CountAsync(),
                query.PageNumber,
                query.PageSize)
            );
    }

    public Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
