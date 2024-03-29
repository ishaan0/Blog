﻿using Blog.Application.Dtos;
using Blog.Domain.IdentityEntities;
using System.Security.Claims;

namespace Blog.Application.ServiceContracts
{
    public interface IJwtService
    {
        AuthenticationResponse CreateJwtToken(User user);
        public ClaimsPrincipal? GetPrincipalFromJwtToken(string? token);
    }
}
