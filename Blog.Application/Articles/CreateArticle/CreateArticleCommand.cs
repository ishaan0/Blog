using Blog.Application.Articles.Common;
using Blog.Domain.Enums;
using MediatR;

namespace Blog.Application.Articles.CreateArticle;

public class CreateArticleCommand : IRequest<ArticleResponse>
{
    public string Title { get; set; }
    public string Body { get; set; }
    public string? CoverImage { get; set; }
    public ArticleStatus Status { get; set; }
    public Guid AuthorId { get; set; }
}
