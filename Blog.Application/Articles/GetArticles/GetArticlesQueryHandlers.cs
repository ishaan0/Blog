using AutoMapper;
using Blog.Application.Articles.Common;
using Blog.Domain.Entities;
using Blog.Domain.Enums;
using Blog.Domain.Interfaces.Persistence.Repositories;
using Blog.Domain.Models;
using Blog.Infrastructure.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Articles.GetArticles;

public class GetArticlesQueryHandlers(IMapper mapper, IGenericRepository<Article> repository) : IRequestHandler<GetArticlesQuery, PaginatedList<ArticleResponse>>
{

    public async Task<PaginatedList<ArticleResponse>> Handle(
        GetArticlesQuery request,
        CancellationToken cancellationToken)
    {
        var queryable = repository
            .GetByCondition(a => a.Status == ArticleStatus.Public, false, cancellationToken)
            .Sort(a => a.Title, request.SortOrder ?? SortOrder.Ascending);

        var itemsToReturn = await queryable
            .GetPage(request.PageNumber, request.PageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedList<ArticleResponse>(
            mapper.Map<List<ArticleResponse>>(itemsToReturn),
            await queryable.GetPaginationMetadataAsync(
                request.PageNumber,
                request.PageSize));
    }
}
