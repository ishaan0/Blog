using Blog.Domain.Enums;

namespace Blog.Application.Articles.Common;

public class ArticleResponse
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Body { get; set; }
    public string? CoverImage { get; set; }
    public ArticleStatus Status { get; set; }
    public int ViewCount { get; set; }
    public int LikeCount { get; set; }
}
