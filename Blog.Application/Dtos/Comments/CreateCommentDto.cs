namespace Blog.Application.Dtos.Comments;

public class CreateCommentDto
{
    public string Body { get; set; }
    public Guid UserId { get; set; }
    public Guid ArticleId { get; set; }
    public Guid ParentCommentId { get; set; }
}
