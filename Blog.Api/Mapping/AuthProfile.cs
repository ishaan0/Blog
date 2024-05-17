using AutoMapper;
using Blog.Api.Dtos.Auth;
using Blog.Application.Users.Login;
using Blog.Application.Users.Register;

namespace Blog.Api.Mapping;

public class AuthProfile : Profile
{
    public AuthProfile()
    {
        CreateMap<RegisterRequest, RegisterCommand>();

        CreateMap<LoginRequest, LoginCommand>();
    }
}
