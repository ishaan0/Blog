using Blog.Application.Interfaces.Repositories;
using Blog.Domain.Enums;
using Blog.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Articles.UpdateArticle;

public class UpdateArticleCommandHandler(
    IArticleRepository articleRepository
    )
    : IRequestHandler<UpdateArticleCommand>
{
    public async Task Handle(
        UpdateArticleCommand request,
        CancellationToken cancellationToken)
    {
        var article = await articleRepository
                            .GetByCondition(a => a.Id == request.Id, true, cancellationToken)
                            .FirstOrDefaultAsync();
        if (article == null)
        {
            throw new BadRequestException("Invalid Id");
        }

        article.Title = request.Title;
        article.Body = request.Body;
        article.CoverImage = request.CoverImage;
        article.Status = ArticleStatus.Private;

        articleRepository.Update(article);
        await articleRepository.SaveAsync(cancellationToken);
    }
}
