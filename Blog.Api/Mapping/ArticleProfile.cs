using AutoMapper;
using Blog.Api.Dtos.Articles;
using Blog.Application.Articles.CreateArticle;

namespace Blog.Api.Mapping;

public class ArticleProfile : Profile
{
    public ArticleProfile()
    {
        CreateMap<ArticleCreationRequest, CreateArticleCommand>();
    }
}
