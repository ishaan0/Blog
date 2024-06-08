using Blog.Domain.Enums;

namespace Blog.Api.Dtos.Articles;

public class ArticleCreationRequest
{
    public string Title { get; set; }
    public string Body { get; set; }
    public string? CoverImage { get; set; }
    public ArticleStatus Status { get; set; }
    public Guid AuthorId { get; set; }
}
