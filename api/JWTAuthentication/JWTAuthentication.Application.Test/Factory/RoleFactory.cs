using JWTAuthentication.Application.Services;
using JWTAuthentication.Application.Test.Contexts;
using JWTAuthentication.Application.Test.Repositories;
using JWTAuthentication.Domain.Usuarios.Roles;
using JWTAuthentication.Domain.Usuarios.Roles.Service;
using JWTAuthentication.Persistence.Repositories.SQLServer;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;

namespace JWTAuthentication.Application.Test.Factory
{
    public class RoleFactory : ServiceFactory<IRoleService>
    {
        public RoleFactory(Mock<AuthenticationOrganizationContextTest> dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override IRoleService GetServiceInstace(Dictionary<string, IList<object>> datas)
        {
            _dbContext.Setup(udc => udc.Roles).ReturnsDbSet(datas["roles"].OfType<Role>());
            return BuildInstace();
        }

        public override IRoleService GetServiceInstace()
        {
            return BuildInstace();
        }

        protected override IRoleService BuildInstace()
        {
            var roleRepositorio = new RoleRepositoryTest(_dbContext.Object,
                   new Mock<ILogger<RoleRepository>>().Object);

            return new RoleService(roleRepositorio);
        }
    }
}
