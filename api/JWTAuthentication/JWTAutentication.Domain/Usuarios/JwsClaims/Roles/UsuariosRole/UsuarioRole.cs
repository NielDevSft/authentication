using JWTAuthentication.Domain.Core.Models;

namespace JWTAuthentication.Domain.Usuarios.JwsClaims.Roles.UsuariosRole
{

    public class UsuarioRole : Entity<UsuarioRole>
    {
        public int? UsuarioId { get; set; }
        public int? RoleId { get; set; }
        public virtual Role? Role { get; set; }

        public virtual Usuario? Usuario { get; set; }

        public override bool IsValid()
        {
            return true;
        }
    }
}
