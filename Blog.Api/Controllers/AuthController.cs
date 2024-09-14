using AutoMapper;
using Blog.Api.Helpers;
using Blog.Application.Dtos;
using Blog.Application.Dtos.Auth;
using Blog.Application.Interfaces;
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
    private readonly IValidator<RegisterRequestDto> _registerRequestDtoValidator;
    private readonly IValidator<LoginRequestDto> _loginRequestDtoValidator;
    private readonly IMapper _mapper;
    private readonly ISender _mediator;

    public AuthController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        RoleManager<Role> roleManager,
        IJwtService jwtService,
        IValidator<RegisterRequestDto> registerRequestDtoValidator,
        IValidator<LoginRequestDto> loginRequestDtoValidator,
        IMapper mapper,
        ISender mediator)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _jwtService = jwtService;
        _registerRequestDtoValidator = registerRequestDtoValidator;
        _loginRequestDtoValidator = loginRequestDtoValidator;
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse<AuthenticationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequestDto registerRequestDto,
        CancellationToken cancellationToken)
    {
        var validatorResult = await _registerRequestDtoValidator.ValidateAsync(registerRequestDto);
        if (!validatorResult.IsValid)
        {
            var errorMessages = validatorResult.Errors.Select(error => error.ErrorMessage).ToList();
            return ApiResponseHelper.BadRequest("Bad request", errorMessages);
        }

        var registerCommand = _mapper.Map<RegisterCommand>(registerRequestDto);

        var authenticationResponse = await _mediator.Send(registerCommand, cancellationToken);

        return ApiResponseHelper.Success(authenticationResponse, "User registered successfully");
    }


    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<AuthenticationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequestDto request,
        CancellationToken cancellationToken)
    {
        var validatorResult = _loginRequestDtoValidator.Validate(request);
        if (!validatorResult.IsValid)
        {
            var errorMessages = validatorResult.Errors.Select(error => error.ErrorMessage).ToList();
            return ApiResponseHelper.BadRequest("Bad request", errorMessages);
        }

        var loginCommand = _mapper.Map<LoginCommand>(request);

        var authenticationResponse = await _mediator.Send(loginCommand, cancellationToken);

        return ApiResponseHelper.Success(authenticationResponse, "User logged in successfully");
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

        return ApiResponseHelper.Success(authenticationResponse, "User logged in successfully");
    }
}
