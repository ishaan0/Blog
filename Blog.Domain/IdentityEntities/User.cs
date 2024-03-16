using Microsoft.AspNetCore.Identity;

namespace Blog.Domain.IdentityEntities
{
    public class User : IdentityUser<Guid>
    {
        public string Name { get; set; }
        public string? Bio { get; set; }
        public string? ProfileImage { get; set; }
        public UserStatus? Status { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }

    public enum UserStatus
    {
        PENDING,
        APPROVED,
        BLOCKED,
        DECLINED
    }
}
