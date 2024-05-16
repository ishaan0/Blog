namespace Blog.Api.Dtos.Auth;

public class RegisterRequest
{
    public string Name { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
    public string ConfirmPassword { get; init; }
}
