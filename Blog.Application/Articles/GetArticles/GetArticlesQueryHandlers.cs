using AutoMapper;
using Blog.Application.Articles.Common;
using Blog.Domain.Enums;
using Blog.Domain.Models;
using Blog.Infrastructure.Data;
using Blog.Infrastructure.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Articles.GetArticles;

public class GetArticlesQueryHandlers : IRequestHandler<GetArticlesQuery, PaginatedList<ArticleResponse>>
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;
    public GetArticlesQueryHandlers(IMapper mapper, ApplicationDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }
    public async Task<PaginatedList<ArticleResponse>> Handle(
        GetArticlesQuery request,
        CancellationToken cancellationToken)
    {
        var queryable = _context.Articles
            .Where(a => a.Status == ArticleStatus.Public)
            .Sort(a => a.Title, request.SortOrder ?? SortOrder.Ascending);

        var itemsToReturn = await queryable
            .GetPage(request.PageNumber, request.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return new PaginatedList<ArticleResponse>(
            _mapper.Map<List<ArticleResponse>>(itemsToReturn),
            await queryable.GetPaginationMetadataAsync(
                request.PageNumber,
                request.PageSize));
    }
}
