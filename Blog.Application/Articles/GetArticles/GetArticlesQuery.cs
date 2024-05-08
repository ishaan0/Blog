using Blog.Application.Articles.Common;
using Blog.Domain.Enums;
using Blog.Domain.Models;
using MediatR;

namespace Blog.Application.Articles.GetArticles;

public class GetArticlesQuery : IRequest<PaginatedList<ArticleResponse>>
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public SortOrder? SortOrder { get; init; }
    public string? SortColumn { get; init; }
}
