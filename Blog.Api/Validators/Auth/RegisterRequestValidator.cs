using Blog.Api.Dtos.Auth;
using FluentValidation;

namespace Blog.Api.Validators.Auth;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(3, 30)
            .WithMessage("Name is not valid");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Email is not valid");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password are not valid");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .Equal(x => x.Password)
            .WithMessage("Confirm password did not match with password");
    }
}
