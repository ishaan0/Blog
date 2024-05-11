using Microsoft.AspNetCore.Identity;

namespace Blog.Domain.IdentityEntities
{
    public class Role : IdentityRole<Guid>
    {
        public Role() : base() { }
        public Role(string role) : base(role) { }
    }
}
