using AutoMapper;
using Blog.Domain.Entities;
using Blog.Domain.Exceptions;
using Blog.Infrastructure.Data;
using FluentValidation;
using MediatR;

namespace Blog.Application.Articles.CreateArticle;

public class CreateArticleCommandHandler(
    IMapper mapper,
    IValidator<CreateArticleCommand> createArticleCommandValidator,
    ApplicationDbContext context
    ) : IRequestHandler<CreateArticleCommand, CreateArticleResponse>
{
    public async Task<CreateArticleResponse> Handle(
        CreateArticleCommand request,
        CancellationToken cancellationToken)
    {
        var validatorResult = await createArticleCommandValidator.ValidateAsync(request);

        if (!validatorResult.IsValid)
        {
            var errorMessage = string.Join(" | ", validatorResult.Errors.Select(error => error));
            throw new BadRequestException(errorMessage);
        }

        Article article = mapper.Map<Article>(request);

        var createdArticle = await context.Articles.AddAsync(article);
        await context.SaveChangesAsync();

        return new CreateArticleResponse(createdArticle.Entity.Id);

    }
}
