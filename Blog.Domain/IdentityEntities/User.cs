using Blog.Domain.Entities;
using Blog.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Blog.Domain.IdentityEntities
{
    public class User : IdentityUser<Guid>
    {
        public string Name { get; set; }
        public string? Bio { get; set; }
        public string? ProfileImage { get; set; }
        public UserStatus Status { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }

        public ICollection<Article>? Articles { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Like>? Likes { get; set; }
        public ICollection<Subscription>? Subscriptions { get; set; }
        public ICollection<Subscription>? Publications { get; set; }
    }
}
