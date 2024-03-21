using JWTAuthentication.Domain.Usuarios.JwsClaims;
using JWTAuthentication.Domain.Usuarios.Roles;
using JWTAuthentication.Domain.Usuarios.Roles.RoleJwtClaims;
using JWTAuthentication.Domain.Usuarios.Roles.RoleJwtClaims.Repository;
using JWTAuthentication.Persistence.Abstractions;
using JWTAuthentication.Persistence.Contexts;
using Microsoft.Extensions.Logging;

namespace JWTAuthentication.Persistence.Repositories
{
    public class RoleJwtClaimRepository : Repository<RoleJwtClaim>, IRoleJwtClaimRepository
    {
        public RoleJwtClaimRepository(AuthenticationOrganizationContext context, ILogger<RoleJwtClaimRepository> logger) : base(context, logger)
        {
        }
        public async Task<JwtClaim> FindRoleJwtClaimExisting(List<Role> roles)
        {

            var roleClaimContainers = await FindAllWhereAsync(rjc => roles.Contains(rjc.Role!), "Role", "JwtClaim");

            var claimCounts = roleClaimContainers
            .Select(container => container.JwtClaim!)
            .GroupBy(claim => claim)
            .ToDictionary(group => group.Key, group => group.Count());

            var claimWithSameRoles = claimCounts.ToList().FirstOrDefault(cc => cc.Value == roles.Count).Key;

            if (claimWithSameRoles != null)
            {
                claimWithSameRoles.RoleJwtClaims = roleClaimContainers
                .ToList().FindAll(rc => rc.JwtClaimUuid == claimWithSameRoles.Uuid);
            }

            return claimWithSameRoles;

        }
    }
}
