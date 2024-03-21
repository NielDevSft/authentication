using JWTAuthentication.Domain.Core.Models;
using JWTAuthentication.Domain.Usuarios.Roles.RoleJwtClaims;

namespace JWTAuthentication.Domain.Usuarios.JwsClaims
{
    public class JwtClaim : Entity<JwtClaim>
    {
        public JwtClaim()
        {
            RoleJwtClaims = new HashSet<RoleJwtClaim>();
        }
        public string Subject { get; set; } = null!;

        public virtual ICollection<RoleJwtClaim> RoleJwtClaims { get; set; } = new List<RoleJwtClaim>();
        public override bool IsValid()
        {
            return true;
        }
    }
}
