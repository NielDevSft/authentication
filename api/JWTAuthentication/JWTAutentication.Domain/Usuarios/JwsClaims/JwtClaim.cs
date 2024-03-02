using JWTAuthentication.Domain.Core.Models;
using JWTAuthentication.Domain.Usuarios.JwsClaims.Roles;

namespace JWTAuthentication.Domain.Usuarios.JwsClaims
{
    public class JwtClaim : Entity<JwtClaim>
    {
        public string Subject { get; set; } = null!;

        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
        public override bool IsValid()
        {
            return true;
        }
    }
}
