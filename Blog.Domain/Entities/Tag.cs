namespace Blog.Domain.Entities;

public class Tag : BaseEntity
{
    public string Name { get; set; }
    public Guid ArticleId { get; set; }

    public Article? Article { get; set; }
}
