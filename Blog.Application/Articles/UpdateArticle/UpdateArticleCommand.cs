using MediatR;

namespace Blog.Application.Articles.UpdateArticle;

public class UpdateArticleCommand : IRequest
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public string? CoverImage { get; set; }
}
