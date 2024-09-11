using AutoMapper;
using Blog.Application.Articles.Common;
using Blog.Application.Dtos.Articles;
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
        var articles = await articleRepository.GetArticlesAsync(mapper.Map<GetArticlesDto>(request), false);

        return new PaginatedList<ArticleResponse>(
           mapper.Map<List<ArticleResponse>>(articles),
           new PaginationMetadata(
               articles.Count(), request.PageNumber, request.PageSize));


    }
}
