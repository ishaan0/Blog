using Blog.Application.Articles.Common;
using Blog.Application.Articles.GetArticles;
using Blog.Domain.Entities;
using Blog.Domain.Models;

namespace Blog.Application.Interfaces.Repositories;

public interface IArticleRepository : IGenericRepository<Article>
{
    Task<PaginatedList<ArticleResponse>> GetArticlesAsync(
        GetArticlesQuery getArticlesQuery,
        bool trackChanges,
        CancellationToken cancellationToken = default);
}
