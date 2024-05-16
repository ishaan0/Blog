using Blog.Application.Users.Common;
using MediatR;

namespace Blog.Application.Users.Register;

public class RegisterCommand : IRequest<AuthenticationResponse>
{
    public string Name { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
    public string ConfirmPassword { get; init; }
}
