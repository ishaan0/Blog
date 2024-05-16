using AutoMapper;
using Blog.Application.ServiceContracts;
using Blog.Application.Users.Common;
using Blog.Domain.Enums;
using Blog.Domain.Exceptions;
using Blog.Domain.IdentityEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Blog.Application.Users.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthenticationResponse>
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IMapper _mapper;
    private readonly IJwtService _jwtService;

    public RegisterCommandHandler(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        RoleManager<Role> roleManager,
        IMapper mapper,
        IJwtService jwtService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _mapper = mapper;
        _jwtService = jwtService;
    }
    public async Task<AuthenticationResponse> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken = default)
    {
        bool validRole = await _roleManager.RoleExistsAsync(nameof(UserType.User));
        if (!validRole) throw new BadRequestException("User role doen't exist");

        if (await _userManager.FindByEmailAsync(request.Email) is not null)
            throw new BadRequestException("Another user with same email exists");

        User user = _mapper.Map<User>(request);
        IdentityResult result = await _userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);

            var authenticationResponse = await _jwtService.CreateJwtToken(user);
            user.RefreshToken = authenticationResponse.RefreshToken;
            user.RefreshTokenExpiration = authenticationResponse.RefreshTokenExpiration;

            await _userManager.UpdateAsync(user);
            await _userManager.AddToRoleAsync(user, nameof(UserType.User));

            return authenticationResponse;
        }
        else
        {
            var errorMessage = string.Join(" | ", result.Errors.Select(e => e.Description));
            throw new CustomException(errorMessage);
        }
    }
}
