using Blog.Application.Dtos.Auth;
using FluentValidation;

namespace Blog.Application.Validators.Auth;

public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
{
    public LoginRequestDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Valid email required");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required");
    }
}
