using Blog.Domain.Enums;
using Blog.Domain.IdentityEntities;

namespace Blog.Domain.Entities;

public class Article : BaseEntity
{
    public string Title { get; set; }
    public string Body { get; set; }
    public string? CoverImage { get; set; }
    public ArticleStatus Status { get; set; }
    public Guid AuthorId { get; set; }

    public User? Author { get; set; }
    public ICollection<Comment>? Comments { get; set; }
    public ICollection<Like>? Likes { get; set; }
    public ICollection<Tag>? Tags { get; set; }
}
