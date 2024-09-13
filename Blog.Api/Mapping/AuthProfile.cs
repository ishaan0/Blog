using AutoMapper;
using Blog.Application.Dtos.Auth;
using Blog.Application.Users.Login;
using Blog.Application.Users.Register;

namespace Blog.Api.Mapping;

public class AuthProfile : Profile
{
    public AuthProfile()
    {
        CreateMap<RegisterRequestDto, RegisterCommand>();

        CreateMap<LoginRequestDto, LoginCommand>();
    }
}
