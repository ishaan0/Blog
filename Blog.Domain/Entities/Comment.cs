using Blog.Domain.IdentityEntities;

namespace Blog.Domain.Entities;

public class Comment : BaseEntity
{
    public string? Body { get; set; }
    public Guid? UserId { get; set; }
    public User? User { get; set; }
    public Guid? ArticleId { get; set; }
    public Article? Article { get; set; }
    public Guid? ParentCommentId { get; set; }
}
