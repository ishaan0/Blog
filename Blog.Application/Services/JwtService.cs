using Blog.Application.ServiceContracts;
using Blog.Application.Users.Common;
using Blog.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Blog.Application.Services;

public class JwtService : IJwtService
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    public JwtService(IConfiguration configuration, UserManager<User> userManager)
    {
        _userManager = userManager;
        _configuration = configuration;
    }
    public async Task<AuthenticationResponse> CreateJwtToken(User user)
    {
        DateTime expiration = DateTime.UtcNow.AddMinutes(
            Convert.ToDouble(_configuration["Jwt:EXPIRATION_MINUTES"]));

        List<Claim> claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Email),
            new Claim(ClaimTypes.Name, user.Name)
        };

        var userRoles = await _userManager.GetRolesAsync(user);
        userRoles.ToList().ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));

        return new AuthenticationResponse()
        {
            Token = GenerateAccessToken(claims, expiration),
            Email = user.Email,
            Name = user.Name,
            RefreshToken = GenerateRefreshToken(),
            Expiration = expiration,
            RefreshTokenExpiration = DateTime.Now.AddDays(
                Convert.ToInt32(_configuration["RefreshToken:EXPIRATION_MINUTES"]))
        };
    }

    private string GenerateAccessToken(List<Claim> claims, DateTime expiration)
    {
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

        SigningCredentials signingCredentials = new SigningCredentials(
            securityKey, SecurityAlgorithms.HmacSha256);


        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            Expires = expiration,
            SigningCredentials = signingCredentials,
            Subject = new ClaimsIdentity(claims)
        };

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        byte[] bytes = new byte[64];
        var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }

    public ClaimsPrincipal? GetPrincipalFromJwtToken(string? token)
    {
        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateAudience = true,
            ValidAudience = _configuration["Jwt:Audience"],
            ValidateIssuer = true,
            ValidIssuer = _configuration["Jwt:Issuer"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
            ValidateLifetime = false
        };

        JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

        ClaimsPrincipal principal = jwtSecurityTokenHandler.ValidateToken(
            token, tokenValidationParameters, out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }
}
