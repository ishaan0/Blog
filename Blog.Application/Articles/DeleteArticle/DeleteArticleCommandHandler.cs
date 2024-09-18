using Blog.Application.Interfaces.Repositories;
using Blog.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Articles.DeleteArticle;

public class DeleteArticleCommandHandler(
    IArticleRepository articleRepository)
    : IRequestHandler<DeleteArticleCommand>
{
    public async Task Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
    {
        var article = await articleRepository
                            .GetByCondition(a => a.Id == request.Id, true, cancellationToken)
                            .FirstOrDefaultAsync(cancellationToken);

        if (article == null)
        {
            throw new BadRequestException("Invalid Id");
        }

        articleRepository.Delete(article, cancellationToken);
        await articleRepository.SaveAsync(cancellationToken);
    }
}
