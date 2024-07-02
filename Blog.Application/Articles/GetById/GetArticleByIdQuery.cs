using Blog.Application.Articles.Common;
using MediatR;

namespace Blog.Application.Articles.GetById;

public class GetArticleByIdQuery : IRequest<ArticleResponse>
{
    public Guid Id { get; set; }
}
