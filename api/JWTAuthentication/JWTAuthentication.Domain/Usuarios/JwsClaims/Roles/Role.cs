using JWTAuthentication.Domain.Core.Models;
using JWTAuthentication.Domain.Usuarios.JwsClaims.Roles.UsuariosRole;

namespace JWTAuthentication.Domain.Usuarios.JwsClaims.Roles
{
    public class Role : Entity<Role>
    {
        public string Name { get; set; } = null!;
        public virtual ICollection<RoleJwtClaim> RoleJwtClaims { get; set; } = new List<RoleJwtClaim>();
        public override bool IsValid()
        {
            return true;
        }
    }
}
