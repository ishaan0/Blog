using AutoMapper;
using Blog.Application.Articles.CreateArticle;
using Blog.Application.Dtos.Comments;
using Blog.Domain.Entities;

namespace Blog.Application.Mapping;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<CreateCommentDto, CreateArticleCommand>();
        CreateMap<CreateArticleCommand, Comment>();
    }
}
