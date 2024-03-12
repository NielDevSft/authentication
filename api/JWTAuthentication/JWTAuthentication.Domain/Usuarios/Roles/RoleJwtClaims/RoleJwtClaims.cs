using JWTAuthentication.Domain.Core.Models;
using JWTAuthentication.Domain.Usuarios.JwsClaims;

namespace JWTAuthentication.Domain.Usuarios.Roles.RoleJwtClaims
{

    public class RoleJwtClaim : Entity<RoleJwtClaim>
    {
        public int? JwtClaimId { get; set; }
        public int? RoleId { get; set; }
        public virtual Role? Role { get; set; }

        public virtual JwtClaim? JwtClaim { get; set; }

        public override bool IsValid()
        {
            return true;
        }
    }
}
