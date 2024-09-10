using AutoMapper;
using Blog.Application.Articles.Common;
using Blog.Application.Articles.CreateArticle;
using Blog.Application.Articles.GetArticles;
using Blog.Application.Dtos.Articles;
using Blog.Domain.Entities;

namespace Blog.Application.Mapping;

public class ArticleProfile : Profile
{
    public ArticleProfile()
    {
        CreateMap<GetArticlesDto, GetArticlesQuery>().ReverseMap();

        CreateMap<Article, ArticleResponse>();

        CreateMap<CreateArticleCommand, Article>();
    }
}
