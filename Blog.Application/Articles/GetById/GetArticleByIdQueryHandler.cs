using AutoMapper;
using Blog.Application.Articles.Common;
using Blog.Application.Interfaces.Repositories;
using Blog.Domain.Entities;
using Blog.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Articles.GetById;

public class GetArticleByIdQueryHandler(
    IMapper mapper,
    IGenericRepository<Article> repository
    ) : IRequestHandler<GetArticleByIdQuery, ArticleResponse>
{
    public async Task<ArticleResponse> Handle(GetArticleByIdQuery request, CancellationToken cancellationToken)
    {
        var articles = await repository.GetByCondition(
            a => a.Id == request.Id, false,
            cancellationToken).ToListAsync();

        if (articles == null || articles.Count == 0)
            throw new NotFoundException("Invalid id");

        return mapper.Map<ArticleResponse>(articles[0]);
    }
}
