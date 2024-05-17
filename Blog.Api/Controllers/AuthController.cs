using AutoMapper;
using Blog.Api.Dtos.Auth;
using Blog.Application.Dtos;
using Blog.Application.ServiceContracts;
using Blog.Application.Users.Common;
using Blog.Application.Users.Login;
using Blog.Application.Users.Register;
using Blog.Domain.Exceptions;
using Blog.Domain.IdentityEntities;
using Blog.Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Blog.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IJwtService _jwtService;
    private readonly IValidator<RegisterRequest> _registerRequestValidator;
    private readonly IMapper _mapper;
    private readonly ISender _mediator;

    public AuthController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        RoleManager<Role> roleManager,
        IJwtService jwtService,
        IValidator<RegisterRequest> registerRequestValidator,
        IMapper mapper,
        ISender mediator)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _jwtService = jwtService;
        _registerRequestValidator = registerRequestValidator;
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse<AuthenticationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<AuthenticationResponse>>> Register(
        RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var registerCommand = _mapper.Map<RegisterCommand>(request);

        var authenticationResponse = await _mediator.Send(registerCommand, cancellationToken);

        return Ok(new ApiResponse<AuthenticationResponse>(
                true, 200,
                "User registered successfully",
                new List<AuthenticationResponse> { authenticationResponse }));
    }


    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<AuthenticationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<AuthenticationResponse>>> Login(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        var loginCommand = _mapper.Map<LoginCommand>(request);

        AuthenticationResponse authenticationResponse = await _mediator.Send(loginCommand, cancellationToken);

        return Ok(new ApiResponse<AuthenticationResponse>(
                true, 200,
                "User registered successfully",
                new List<AuthenticationResponse> { authenticationResponse }));
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return NoContent();
    }

    [HttpGet("refreshToken")]
    public async Task<IActionResult> RefreshToken(TokenDTO tokenDTO)
    {
        if (tokenDTO == null)
        {
            return BadRequest("Invalid client request");
        }

        ClaimsPrincipal? principal = _jwtService.GetPrincipalFromJwtToken(tokenDTO.Token);
        if (principal == null)
        {
            throw new BadRequestException("Invalid jwt access token");
        }

        string? email = principal.FindFirstValue(ClaimTypes.Email);

        User? user = await _userManager.FindByEmailAsync(email);

        if (user == null || user.RefreshToken != tokenDTO.RefreshToken || user.RefreshTokenExpiration <= DateTime.Now)
        {
            throw new BadRequestException("Invalid refresh token");
        }

        AuthenticationResponse authenticationResponse = await _jwtService.CreateJwtToken(user);

        user.RefreshToken = authenticationResponse.RefreshToken;
        user.RefreshTokenExpiration = authenticationResponse.RefreshTokenExpiration;

        await _userManager.UpdateAsync(user);

        return Ok(new ApiResponse<AuthenticationResponse>(
                true, 200,
                "User logged in successfully",
                new List<AuthenticationResponse> { authenticationResponse }));
    }
}
