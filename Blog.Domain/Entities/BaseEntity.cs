namespace Blog.Domain.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime? CreatedAtUtc { get; set; }
    public DateTime? ModifiedAtUtc { get; set; }
}
