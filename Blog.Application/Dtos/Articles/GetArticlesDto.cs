using Blog.Application.Dtos.Common;

namespace Blog.Application.Dtos.Articles;

public class GetArticlesDto : ResourceQueryDto
{
    public string? Tag { get; set; }
    public Guid? OwnerId { get; set; }
    public string? Search { get; set; }
    public bool? Published { get; set; }
}
