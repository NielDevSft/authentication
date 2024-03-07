using JWTAuthentication.Domain.Usuarios.Roles.RoleJwtClaims;
using JWTAuthentication.Domain.Usuarios.Roles.RoleJwtClaims.Repository;
using JWTAuthentication.Persistence.Abstractions;
using JWTAuthentication.Persistence.Contexts;
using Microsoft.Extensions.Logging;

namespace JWTAuthentication.Persistence.Repositories
{
    public class RoleJwtClaimRepository : Repository<RoleJwtClaim>, IRoleJwtClaimRepository
    {
        public RoleJwtClaimRepository(AuthenticationOrganizationContext context, ILogger<RoleRepository> logger) : base(context, logger)
        {
        }
    }
}
