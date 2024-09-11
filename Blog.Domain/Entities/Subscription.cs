using Blog.Domain.IdentityEntities;

namespace Blog.Domain.Entities;

public class Subscription : BaseEntity
{
    public Guid SubscriberId { get; set; }
    public Guid PublisherId { get; set; }
    public DateTime SubscribedAtUtc { get; set; }

    public User? Subscriber { get; set; }
    public User? Publisher { get; set; }
}
