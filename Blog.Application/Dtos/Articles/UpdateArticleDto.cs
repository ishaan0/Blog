namespace Blog.Application.Dtos.Articles;

public class UpdateArticleDto
{
    public string Title { get; set; }
    public string Body { get; set; }
    public string? CoverImage { get; set; }
}
