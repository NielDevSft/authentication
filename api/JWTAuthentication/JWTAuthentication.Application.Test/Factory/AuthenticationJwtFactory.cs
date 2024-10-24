using JWTAuthentication.Application.Authentications;
using JWTAuthentication.Application.Services;
using JWTAuthentication.Application.SetupOptions;
using JWTAuthentication.Application.Test.Contexts;
using JWTAuthentication.Application.Test.Repositories;
using JWTAuthentication.Domain.Authentications.Services;
using JWTAuthentication.Domain.Usuarios;
using JWTAuthentication.Domain.Usuarios.Roles;
using JWTAuthentication.Domain.Usuarios.Roles.RoleJwtClaims;
using JWTAuthentication.Persistence.Repositories.SQLServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;

namespace JWTAuthentication.Application.Test.Factory
{
    public class AuthenticationJwtFactory : ServiceFactory<IAuthenticationJwtService>
    {

        public AuthenticationJwtFactory(Mock<AuthenticationOrganizationContextTest> dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override IAuthenticationJwtService GetServiceInstace(Dictionary<string, IList<object>> datas)
        {

            _dbContext.Setup(db => db.Usuarios).ReturnsDbSet(datas["usuarios"].OfType<Usuario>());
            _dbContext.Setup(db => db.RoleJwtClaims).ReturnsDbSet(datas["roleJwtClaims"].OfType<RoleJwtClaim>());
            _dbContext.Setup(db => db.Roles).ReturnsDbSet(datas["roles"].OfType<Role>());

            return BuildInstace();
        }

        public override IAuthenticationJwtService GetServiceInstace()
        {
            return BuildInstace();
        }

        protected override IAuthenticationJwtService BuildInstace()
        {
            var usuarioRepositorio = new UsuarioRepositoryTest(_dbContext.Object,
                new Mock<ILogger<UsuarioRepository>>().Object);
            var options = new JwtOptions()
            {
                Issuer = "AuthenticationAPITest",
                Audience = "MicroUsersAuthenticationAPITest",
                SecretKey = "TestKey23423423@#$@#$@#$342342342342342342344563543534534534543534534534534534534"
            };
            var jwtBearerOptions = new JwtBeareOptionsSetup(options);
            jwtBearerOptions.Configure(new JwtBearerOptions());

            var jwtProvider = new JwtProvider(options, jwtBearerOptions);
            var roleJwtClaimRepositorio = new RoleJwtClaimRepositoryTest(_dbContext.Object,
                new Mock<ILogger<RoleJwtClaimRepository>>().Object);

            return new AuthenticationService(usuarioRepositorio, roleJwtClaimRepositorio, jwtProvider);
        }
    }
}
