using Blog.Application.Users.Common;
using MediatR;

namespace Blog.Application.Users.Login;

public class LoginCommand: IRequest<AuthenticationResponse>
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
