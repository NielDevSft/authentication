using JWTAuthentication.Domain.Core.Models;
using JWTAuthentication.Domain.Usuarios.JwsClaims.Roles.UsuariosRole;

namespace JWTAuthentication.Domain.Usuarios.JwsClaims.Roles
{
    public class Role : Entity<Role>
    {
        public int? JwtClaimsId { get; set; }
        public string Name { get; set; } = null!;
        public virtual ICollection<UsuarioRole> UserRoles { get; set; } = new List<UsuarioRole>();
        public override bool IsValid()
        {
            return true;
        }
    }
}
