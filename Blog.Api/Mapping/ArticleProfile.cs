using AutoMapper;
using Blog.Application.Articles.CreateArticle;
using Blog.Application.Dtos.Articles;

namespace Blog.Api.Mapping;

public class ArticleProfile : Profile
{
    public ArticleProfile()
    {
        CreateMap<CreateArticleDto, CreateArticleCommand>();
    }
}
