using JWTAuthentication.Domain.Core.Interfaces;
using JWTAuthentication.Domain.Usuarios.JwsClaims;

namespace JWTAuthentication.Domain.Usuarios.Roles.RoleJwtClaims.Repository
{
    public interface IRoleJwtClaimRepository : IRepository<RoleJwtClaim>
    {
        public Task<JwtClaim> FindRoleJwtClaimExisting(List<Role> roles);
    }
}
