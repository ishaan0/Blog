using Blog.Application.Dtos;
using Blog.Application.ServiceContracts;
using Blog.Domain.Enums;
using Blog.Domain.Exceptions;
using Blog.Domain.IdentityEntities;
using Blog.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Blog.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IJwtService _jwtService;

    public AccountController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        RoleManager<Role> roleManager,
        IJwtService jwtService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _jwtService = jwtService;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse<AuthenticationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<User>> Register(RegisterDTO registerDTO)
    {
        if (!ModelState.IsValid)
        {
            var errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return Problem(errorMessage);
        }

        User user = new User()
        {
            Name = registerDTO.Name,
            UserName = registerDTO.Email,
            Email = registerDTO.Email
        };

        bool validRole = await _roleManager.RoleExistsAsync(nameof(UserType.User));
        if (!validRole)
        {
            throw new BadRequestException("User role doen't exist");
        }

        IdentityResult result = await _userManager.CreateAsync(user, registerDTO.Password);

        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
            var authenticationResponse = await _jwtService.CreateJwtToken(user);
            user.RefreshToken = authenticationResponse.RefreshToken;
            user.RefreshTokenExpiration = authenticationResponse.RefreshTokenExpiration;
            await _userManager.UpdateAsync(user);
            await _userManager.AddToRoleAsync(user, nameof(UserType.User));
            return Ok(new ApiResponse<AuthenticationResponse>(
                true, 200,
                "User registered successfully",
                new List<AuthenticationResponse> { authenticationResponse }));
        }
        else
        {
            var errorMessage = string.Join(" | ", result.Errors.Select(e => e.Description));
            throw new CustomException(errorMessage);
        }
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<AuthenticationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login(LoginDTO loginDTO)
    {
        if (!ModelState.IsValid)
        {
            string errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            throw new BadRequestException(errorMessage);
        }

        var result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, isPersistent: false, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            User? user = await _userManager.FindByEmailAsync(loginDTO.Email);
            var authenticationResponse = await _jwtService.CreateJwtToken(user);
            user.RefreshToken = authenticationResponse.RefreshToken;
            user.RefreshTokenExpiration = authenticationResponse.RefreshTokenExpiration;
            await _userManager.UpdateAsync(user);
            return Ok(new ApiResponse<AuthenticationResponse>(
                true, 200,
                "User logged in successfully",
                new List<AuthenticationResponse> { authenticationResponse }));
        }
        else
        {
            throw new BadRequestException("Invalid email or password");
        }

    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return NoContent();
    }

    [HttpGet("isEmailAlreadyRegistered")]
    public async Task<IActionResult> IsEmailAlreadyRegistered(string email)
    {
        User? user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            return Ok(true);
        }
        else
        {
            return Ok(false);
        }
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
