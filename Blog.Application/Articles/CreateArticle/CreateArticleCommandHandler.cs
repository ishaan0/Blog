using AutoMapper;
using Blog.Application.Interfaces.Repositories;
using Blog.Domain.Entities;
using MediatR;

namespace Blog.Application.Articles.CreateArticle;

public class CreateArticleCommandHandler(
    IMapper mapper,
    IArticleRepository articleRepository
    ) : IRequestHandler<CreateArticleCommand, CreateArticleResponse>
{
    public async Task<CreateArticleResponse> Handle(
        CreateArticleCommand request,
        CancellationToken cancellationToken)
    {
        Article article = mapper.Map<Article>(request);

        articleRepository.Create(article);
        await articleRepository.SaveAsync();

        return new CreateArticleResponse(article.Id);
    }
}
