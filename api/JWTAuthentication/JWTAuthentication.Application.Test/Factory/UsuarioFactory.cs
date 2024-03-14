using JWTAuthentication.Application.Services;
using JWTAuthentication.Application.Test.Contexts;
using JWTAuthentication.Application.Test.Repositories;
using JWTAuthentication.Domain.Usuarios;
using JWTAuthentication.Domain.Usuarios.Roles;
using JWTAuthentication.Domain.Usuarios.Roles.RoleJwtClaims;
using JWTAuthentication.Domain.Usuarios.Service;
using JWTAuthentication.Persistence.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;

namespace JWTAuthentication.Application.Test.Factory
{
    public class UsuarioFactory : ServiceFactory<IUsuarioService>
    {
        public UsuarioRepositoryTest usuarioRepositorioTest;
        public RoleJwtClaimRepositoryTest roleJwtClaimRepositoryTest;
        public RoleRepositoryTest roleRepositoryTest;
        public UsuarioFactory(Mock<AuthenticationOrganizationContextTest> dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override IUsuarioService GetServiceInstace(Dictionary<string, IList<dynamic>> datas)
        {
            _dbContext.Setup(udc => udc.Usuarios).ReturnsDbSet(datas["usuarios"].OfType<Usuario>());
            _dbContext.Setup(udc => udc.Roles).ReturnsDbSet(datas["roles"].OfType<Role>());
            _dbContext.Setup(udc => udc.RoleJwtClaims).ReturnsDbSet(datas["roleJwtClaims"].OfType<RoleJwtClaim>());

            return BuildInstace();
        }
        public override IUsuarioService GetServiceInstace()
        {
            return BuildInstace();
        }

        protected override IUsuarioService BuildInstace()
        {

            usuarioRepositorioTest = usuarioRepositorioTest ?? new UsuarioRepositoryTest(_dbContext.Object,
                new Mock<ILogger<UsuarioRepository>>().Object);

            roleRepositoryTest = roleRepositoryTest ?? new RoleRepositoryTest(_dbContext.Object,
                new Mock<ILogger<RoleRepository>>().Object);

            roleJwtClaimRepositoryTest = roleJwtClaimRepositoryTest ?? new RoleJwtClaimRepositoryTest(_dbContext.Object,
                new Mock<ILogger<RoleJwtClaimRepository>>().Object);

            return new UsuarioService(usuarioRepositorioTest, roleRepositoryTest, roleJwtClaimRepositoryTest);
        }
    }
}
