using Blog.Application.Articles.CreateArticle;
using FluentValidation;

namespace Blog.Application.Validators.Article;

public class CreateArticleCommandValidator : AbstractValidator<CreateArticleCommand>
{
    public CreateArticleCommandValidator()
    {

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.Body)
            .NotEmpty();

        RuleFor(x => x.Status)
            .IsInEnum();

        RuleFor(x => x.AuthorId)
            .NotEmpty()
            .MustAsync(BeValidUser)
            .WithMessage("Author id is not valid");
    }

    private async Task<bool> BeValidUser(Guid authorId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
        //return await _context.Users.AnyAsync(u => u.Id == authorId, cancellationToken);
    }
}
