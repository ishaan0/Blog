using MediatR;

namespace Blog.Application.Comments.CreateComment;

public class CreateCommentCommand : IRequest<Guid>
{
    public string Body { get; set; }
    public Guid UserId { get; set; }
    public Guid ArticleId { get; set; }
    public Guid ParentCommentId { get; set; }
}
