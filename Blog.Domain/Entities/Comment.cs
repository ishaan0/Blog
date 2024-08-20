using Blog.Domain.IdentityEntities;

namespace Blog.Domain.Entities;

public class Comment : BaseEntity
{
    public string Body { get; set; }
    public Guid UserId { get; set; }
    public Guid ArticleId { get; set; }
    public Guid? ParentCommentId { get; set; }

    public User? User { get; set; }
    public Article? Article { get; set; }
    public Comment? ParentComment { get; set; }
    public ICollection<Comment>? Replies { get; set; }
    public ICollection<Like>? Likes { get; set; }

}
