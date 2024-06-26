﻿using AutoMapper;
using Blog.Application.Articles.Common;
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
    ) : IRequestHandler<CreateArticleCommand, ArticleResponse>
{
    public async Task<ArticleResponse> Handle(
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

        return mapper.Map<ArticleResponse>(createdArticle.Entity);
    }
}
