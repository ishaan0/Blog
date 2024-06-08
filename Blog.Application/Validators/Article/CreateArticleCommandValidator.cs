using Blog.Application.Articles.CreateArticle;
using Blog.Infrastructure.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Validators.Article;

public class CreateArticleCommandValidator : AbstractValidator<CreateArticleCommand>
{
    private ApplicationDbContext _context;
    public CreateArticleCommandValidator(ApplicationDbContext context)
    {
        _context = context;

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
        return await _context.Users.AnyAsync(u => u.Id == authorId, cancellationToken);
    }
}
