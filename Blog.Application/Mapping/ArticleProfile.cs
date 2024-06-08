using AutoMapper;
using Blog.Application.Articles.Common;
using Blog.Application.Articles.CreateArticle;
using Blog.Domain.Entities;

namespace Blog.Application.Mapping;

public class ArticleProfile : Profile
{
    public ArticleProfile()
    {
        CreateMap<Article, ArticleResponse>();

        CreateMap<CreateArticleCommand, Article>();
    }
}
