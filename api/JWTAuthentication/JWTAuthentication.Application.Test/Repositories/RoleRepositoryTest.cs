using JWTAuthentication.Application.Test.Contexts;
using JWTAuthentication.Persistence.Repositories.SQLServer;
using Microsoft.Extensions.Logging;

namespace JWTAuthentication.Application.Test.Repositories
{
    public class RoleRepositoryTest : RoleRepository
    {
        public RoleRepositoryTest(AuthenticationOrganizationContextTest context, ILogger<RoleRepository> logger) : base(context, logger)
        {
            DbSet = context.Roles;
        }
    }
}
