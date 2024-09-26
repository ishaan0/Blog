using AutoMapper;
using Blog.Application.Interfaces.Repositories;
using Blog.Domain.Entities;
using Blog.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Comments.CreateComment;

public class CreateCommentCommandHandler(
    IUserRepository userRepository,
    IArticleRepository articleRepository,
    ICommentRepository commentRepository,
    IMapper mapper
    ) : IRequestHandler<CreateCommentCommand, Guid>
{
    public async Task<Guid> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        if (!(await userRepository.AuthorExistAsync(request.UserId, cancellationToken)))
        {
            throw new BadRequestException("Invalid user id");
        }

        if ((await articleRepository.GetByCondition(a => a.Id == request.ArticleId, false, cancellationToken).FirstOrDefaultAsync()) is null)
        {
            throw new BadRequestException("Invalid article id");
        }

        Comment comment = mapper.Map<Comment>(request);

        commentRepository.Create(comment, cancellationToken);
        await commentRepository.SaveAsync(cancellationToken);

        return comment.Id;
    }
}
