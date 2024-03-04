using JWTAuthentication.Domain.Core.Models;

namespace JWTAuthentication.Domain.Usuarios.JwsClaims.Roles.UsuariosRole
{

    public class RoleJwtClaim : Entity<RoleJwtClaim>
    {
        public int? ClaimId { get; set; }
        public int? RoleId { get; set; }
        public virtual Role? Role { get; set; }

        public virtual JwtClaim? JwtClaim { get; set; }

        public override bool IsValid()
        {
            return true;
        }
    }
}
