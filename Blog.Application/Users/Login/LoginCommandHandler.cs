using Blog.Application.ServiceContracts;
using Blog.Application.Users.Common;
using Blog.Domain.Exceptions;
using Blog.Domain.IdentityEntities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Blog.Application.Users.Login;

public class LoginCommandHandler(
        SignInManager<User> signInManager,
        UserManager<User> userManager,
        IJwtService jwtService,
        IValidator<LoginCommand> LoginCommandValidator
    ) : IRequestHandler<LoginCommand, AuthenticationResponse>
{
    public async Task<AuthenticationResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var validatorResult = LoginCommandValidator.Validate(request);
        if (!validatorResult.IsValid)
        {
            var errorMessage = string.Join(" | ", validatorResult.Errors.Select(error => error));
            throw new BadRequestException(errorMessage);
        }

        var result = await signInManager.PasswordSignInAsync(
            request.Email, request.Password,
            isPersistent: false, lockoutOnFailure: false);
        if (!result.Succeeded) throw new BadRequestException("Invalid email or password");

        User? user = await userManager.FindByEmailAsync(request.Email);
        var authenticationResponse = await jwtService.CreateJwtToken(user);
        user.RefreshToken = authenticationResponse.RefreshToken;
        user.RefreshTokenExpiration = authenticationResponse.RefreshTokenExpiration;
        await userManager.UpdateAsync(user);

        return authenticationResponse;
    }
}
