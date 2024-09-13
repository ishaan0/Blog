using AutoMapper;
using Blog.Application.Articles.Common;
using Blog.Application.Interfaces.Repositories;
using Blog.Domain.Models;
using MediatR;

namespace Blog.Application.Articles.GetArticles;

public class GetArticlesQueryHandlers(IMapper mapper, IArticleRepository articleRepository) : IRequestHandler<GetArticlesQuery, PaginatedList<ArticleResponse>>
{

    public async Task<PaginatedList<ArticleResponse>> Handle(
        GetArticlesQuery request,
        CancellationToken cancellationToken)
    {
        var articlePaginatedList = await articleRepository.GetArticlesAsync(request, false);

        return articlePaginatedList;
    }
}
