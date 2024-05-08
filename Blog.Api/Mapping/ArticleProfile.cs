using AutoMapper;
using Blog.Api.Dtos.Articles;
using Blog.Application.Articles.GetArticles;

namespace Blog.Api.Mapping;

public class ArticleProfile : Profile
{
    public ArticleProfile()
    {
        CreateMap<ArticlesGetRequest, GetArticlesQuery>();
    }
}
