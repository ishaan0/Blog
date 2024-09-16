using Blog.Application.Dtos.Articles;
using FluentValidation;

namespace Blog.Application.Validators.Article;

public class UpdateArticleDtoValidator: AbstractValidator<UpdateArticleDto>
{
    public UpdateArticleDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(50).WithMessage("Title cannot exceed 50 characters");

        RuleFor(x => x.Body)
            .NotEmpty().WithMessage("Body cannot be empty");

    }
}
