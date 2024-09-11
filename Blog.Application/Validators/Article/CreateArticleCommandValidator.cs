﻿using Blog.Application.Articles.CreateArticle;
using Blog.Application.Interfaces.Repositories;
using FluentValidation;

namespace Blog.Application.Validators.Article;

public class CreateArticleCommandValidator : AbstractValidator<CreateArticleCommand>
{
    private readonly IUserRepository _userRepository;
    public CreateArticleCommandValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(50).WithMessage("Title cannot exceed 50 characters");

        RuleFor(x => x.Body)
            .NotEmpty().WithMessage("Body cannot be empty");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid article status");

        RuleFor(x => x.AuthorId)
            .NotEmpty().WithMessage("AuthorId cannot be empty")
            .NotEqual(Guid.Empty).WithMessage("Invalid AuthorId")
            .MustAsync(BeValidUser).WithMessage("AuthorId doesn't exist");
    }

    private async Task<bool> BeValidUser(Guid authorId, CancellationToken cancellationToken = default)
    {
        return await _userRepository.AuthorExistAsync(authorId, cancellationToken);
        //return await _context.Users.AnyAsync(u => u.Id == authorId, cancellationToken);
    }
}
