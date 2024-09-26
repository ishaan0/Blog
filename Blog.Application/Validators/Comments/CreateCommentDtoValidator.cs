using Blog.Application.Dtos.Comments;
using FluentValidation;

namespace Blog.Application.Validators.Comments;

public class CreateCommentDtoValidator : AbstractValidator<CreateCommentDto>
{
    public CreateCommentDtoValidator()
    {
        RuleFor(c => c.Body)
            .NotEmpty().WithMessage("Comment body is required");

        RuleFor(c => c.ArticleId)
            .NotEmpty().WithMessage("Article id is required");

        RuleFor(c => c.UserId)
            .NotEmpty().WithMessage("User id is required");

    }
}
