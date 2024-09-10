using Blog.Application.Articles.Common;
using Blog.Application.Dtos.Common;
using Blog.Domain.Models;
using MediatR;

namespace Blog.Application.Articles.GetArticles;

public class GetArticlesQuery : ResourceQueryDto, IRequest<PaginatedList<ArticleResponse>>
{
    public string? Tag { get; init; }
    public Guid? OwnerId { get; init; }
    public string? Search { get; init; }
    public bool? Published { get; init; }
}
