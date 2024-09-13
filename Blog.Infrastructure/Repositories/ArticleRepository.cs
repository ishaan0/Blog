using AutoMapper;
using Blog.Application.Articles.Common;
using Blog.Application.Articles.GetArticles;
using Blog.Application.Extensions;
using Blog.Application.Interfaces.Repositories;
using Blog.Domain.Entities;
using Blog.Domain.Enums;
using Blog.Domain.Models;
using Blog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Repositories;

public class ArticleRepository : GenericRepository<Article>, IArticleRepository
{
    private readonly IMapper _mapper;
    public ArticleRepository(ApplicationDbContext context, IMapper mapper) : base(context)
    {
        _mapper = mapper;
    }

    public async Task<PaginatedList<ArticleResponse>> GetArticlesAsync(
        GetArticlesQuery getArticlesQuery,
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        var query = trackChanges
                        ? _dbSet.AsNoTracking().AsQueryable()
                        : _context.Articles.AsQueryable();

        if (!string.IsNullOrEmpty(getArticlesQuery.Tag))
        {
            query = query.Where(a => a.Tags.Any(t => t.Name == getArticlesQuery.Tag));
        }

        if (getArticlesQuery.OwnerId.HasValue)
        {
            query = query.Where(a => a.AuthorId == getArticlesQuery.OwnerId.Value);
        }

        if (!string.IsNullOrEmpty(getArticlesQuery.Search))
        {
            query = query.Where(a => a.Title.Contains(getArticlesQuery.Search) || a.Body.Contains(getArticlesQuery.Search));
        }

        if (getArticlesQuery.Published.HasValue)
        {
            query = getArticlesQuery.Published.Value
                ? query.Where(a => a.Status == ArticleStatus.Public)
                : query.Where(a => a.Status == ArticleStatus.Private);
        }

        query = query.Sort(a => a.Title, getArticlesQuery.SortOrder ?? SortOrder.Ascending);

        return await query.ToPaginatedList<Article, ArticleResponse>(
                getArticlesQuery.PageNumber, getArticlesQuery.PageSize,
                article => _mapper.Map<ArticleResponse>(article));
    }

}
