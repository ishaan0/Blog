using AutoMapper;
using Blog.Application.Users.Register;
using Blog.Domain.IdentityEntities;

namespace Blog.Application.Mapping;

public class UsersProfile : Profile
{
    public UsersProfile()
    {
        //CreateMap<RegisterCommand, User>()
        //    .ForMember(dst => dst.UserName, options => options.MapFrom(src => src.Email));
        CreateMap<RegisterCommand, User>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.Bio, opt => opt.Ignore())
            .ForMember(dest => dest.ProfileImage, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.Articles, opt => opt.Ignore())
            .ForMember(dest => dest.Comments, opt => opt.Ignore())
            .ForMember(dest => dest.RefreshToken, opt => opt.Ignore())
            .ForMember(dest => dest.RefreshTokenExpiration, opt => opt.Ignore());
    }
}
