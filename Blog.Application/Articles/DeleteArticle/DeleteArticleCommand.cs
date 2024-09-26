using MediatR;

namespace Blog.Application.Articles.DeleteArticle;

public class DeleteArticleCommand : IRequest
{
    public Guid Id { get; set; }
}
