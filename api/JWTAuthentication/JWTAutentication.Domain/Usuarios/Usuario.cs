using JWTAuthentication.Domain.Core.Models;
using JWTAuthentication.Domain.Usuarios.JwsClaims.Roles.UsuariosRole;

namespace JWTAuthentication.Domain.Usuarios
{
    public class Usuario : Entity<Usuario>
    {
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Username { get; set; } = null!;

        public virtual ICollection<UsuarioRole> UsuarioRoles { get; set; } = new List<UsuarioRole>();
        public override bool IsValid()
        {
            return true;
        }
    }
}
