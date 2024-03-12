using JWTAuthentication.Application.Test.Contexts;
using JWTAuthentication.Persistence.Repositories;
using Microsoft.Extensions.Logging;

namespace JWTAuthentication.Application.Test.Repositories
{
    internal class RoleJwtClaimRepositoryTest : RoleJwtClaimRepository
    {
        public RoleJwtClaimRepositoryTest(AuthenticationOrganizationContextTest context, ILogger<RoleJwtClaimRepository> logger) : base(context, logger)
        {
        }
    }
}
