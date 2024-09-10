using Blog.Application.Dtos.Articles;
using Blog.Application.Extensions;
using Blog.Application.Interfaces.Repositories;
using Blog.Domain.Entities;
using Blog.Domain.Enums;
using Blog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Repositories;

public class ArticleRepository : GenericRepository<Article>, IArticleRepository
{
    public ArticleRepository(ApplicationDbContext context) : base(context)
    { }

    public async Task<IEnumerable<Article>> GetArticlesAsync(
        GetArticlesDto getArticlesDto,
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        var query = trackChanges
                        ? _dbSet.AsNoTracking().AsQueryable()
                        : _context.Articles.AsQueryable();

        if (!string.IsNullOrEmpty(getArticlesDto.Tag))
        {
            query = query.Where(a => a.Tags.Any(t => t.Name == getArticlesDto.Tag));
        }

        if (getArticlesDto.OwnerId.HasValue)
        {
            query = query.Where(a => a.AuthorId == getArticlesDto.OwnerId.Value);
        }

        if (!string.IsNullOrEmpty(getArticlesDto.Search))
        {
            query = query.Where(a => a.Title.Contains(getArticlesDto.Search) || a.Body.Contains(getArticlesDto.Search));
        }

        if (getArticlesDto.Published.HasValue)
        {
            query = getArticlesDto.Published.Value
                ? query.Where(a => a.Status == ArticleStatus.Public)
                : query.Where(a => a.Status == ArticleStatus.Private);
        }

        query = query.Sort(a => a.Title, getArticlesDto.SortOrder ?? SortOrder.Ascending);

        var aritles = await query
            .GetPage(getArticlesDto.PageNumber, getArticlesDto.PageSize)
            .ToListAsync(cancellationToken);

        return aritles;
    }

}
