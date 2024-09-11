using AutoMapper;
using Blog.Application.Interfaces.Repositories;
using Blog.Domain.Entities;
using Blog.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace Blog.Application.Articles.CreateArticle;

public class CreateArticleCommandHandler(
    IMapper mapper,
    IArticleRepository articleRepository,
    IValidator<CreateArticleCommand> createArticleCommandValidator
    ) : IRequestHandler<CreateArticleCommand, CreateArticleResponse>
{
    public async Task<CreateArticleResponse> Handle(
        CreateArticleCommand request,
        CancellationToken cancellationToken)
    {
        var validatorResult = await createArticleCommandValidator.ValidateAsync(request);

        if (!validatorResult.IsValid)
        {
            var errormessage = string.Join(" | ", validatorResult.Errors.Select(error => error));
            throw new BadRequestException(errormessage);
        }

        Article article = mapper.Map<Article>(request);

        articleRepository.Create(article);
        await articleRepository.SaveAsync();

        return new CreateArticleResponse(article.Id);
    }
}
