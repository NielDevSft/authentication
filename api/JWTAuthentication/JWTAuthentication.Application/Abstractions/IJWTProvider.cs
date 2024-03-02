using JWTAuthentication.Domain.Usuarios;
using JWTAuthentication.Domain.Usuarios.JwsClaims.Roles;

namespace JWTAuthentication.Application.Abstractions
{
    public interface IJWTProvider
    {
        public string GenerateToken(Usuario usuario, ICollection<Role> roles);

    }
}
