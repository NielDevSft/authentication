using JWTAuthentication.Domain.Usuarios.JwsClaims.Roles.UsuariosRole;
using JWTAuthentication.Domain.Usuarios.JwsClaims.Roles.UsuariosRole.Repository;
using JWTAuthentication.Domain.Usuarios.Repository;
using JWTAuthentication.Persistence.Abstractions;
using JWTAuthentication.Persistence.Contexts;

namespace JWTAuthentication.Persistence.Repositories
{
    public class UsuarioRoleRepository : Repository<RoleJwtClaim>, IRoleJwtClaimRepository
    {
        public UsuarioRoleRepository(AuthenticationOrganizationContext context) : base(context)
        {
        }
    }
}
