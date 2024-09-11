using Blog.Application.Dtos.Articles;
using Blog.Domain.Entities;

namespace Blog.Application.Interfaces.Repositories;

public interface IArticleRepository : IGenericRepository<Article>
{
    Task<IEnumerable<Article>> GetArticlesAsync(
        GetArticlesDto getArticlesDto,
        bool trackChanges,
        CancellationToken cancellationToken = default);
}
