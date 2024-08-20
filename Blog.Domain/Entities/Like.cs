using Blog.Domain.Enums;
using Blog.Domain.IdentityEntities;

namespace Blog.Domain.Entities;

public class Like : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid TargetId { get; set; }
    public LikeTargetType TargetType { get; set; }

    public User? User { get; set; }
    public Article? Article { get; set; }
    public Comment? Comment { get; set; }
}
