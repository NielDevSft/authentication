using JWTAuthentication.Application.Test.Contexts;
using JWTAuthentication.Persistence.Repositories.SQLServer;
using Microsoft.Extensions.Logging;

namespace JWTAuthentication.Application.Test.Repositories
{
    public class RoleJwtClaimRepositoryTest : RoleJwtClaimRepository
    {
        public RoleJwtClaimRepositoryTest(AuthenticationOrganizationContextTest context, ILogger<RoleJwtClaimRepository> logger) : base(context, logger)
        {
            DbSet = context.RoleJwtClaims;
        }
    }
}
