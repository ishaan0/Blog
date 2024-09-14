using Blog.Domain.Enums;

namespace Blog.Application.Dtos.Articles;

public class CreateArticleDto
{
    public string Title { get; set; }
    public string Body { get; set; }
    public string? CoverImage { get; set; }
    public ArticleStatus Status { get; set; }
    public Guid AuthorId { get; set; }
}
