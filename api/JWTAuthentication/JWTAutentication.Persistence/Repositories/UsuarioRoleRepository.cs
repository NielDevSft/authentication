using JWTAuthentication.Domain.Usuarios.JwsClaims.Roles.UsuariosRole;
using JWTAuthentication.Persistence.Abstractions;
using JWTAuthentication.Persistence.Contexts;

namespace JWTAuthentication.Persistence.Repositories
{
    internal class UsuarioRoleRepository : Repository<UsuarioRole>
    {
        public UsuarioRoleRepository(AuthenticationOrganizationContext context) : base(context)
        {
        }
    }
}
