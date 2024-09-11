using Blog.Application.Users.Common;
using Blog.Domain.IdentityEntities;
using System.Security.Claims;

namespace Blog.Application.Interfaces;

public interface IJwtService
{
    public Task<AuthenticationResponse> CreateJwtToken(User user);
    public ClaimsPrincipal? GetPrincipalFromJwtToken(string? token);
}
