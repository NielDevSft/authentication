using JWTAuthentication.Application.Services;
using JWTAuthentication.Application.Test.Contexts;
using JWTAuthentication.Application.Test.Repositories;
using JWTAuthentication.Domain.Usuarios;
using JWTAuthentication.Domain.Usuarios.Roles;
using JWTAuthentication.Persistence.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using System.Data;

namespace JWTAuthentication.Application.Test.Factory
{
    public static class ServiceFactory
    {
        public static UsuarioService GetUsuarioService(IList<Usuario> usuarios, IList<Role> roles)
        {
            //criação db context e adição dos db sets
            var usuarioDbContext = new Mock<AuthenticationOrganizationContextTest>();
            var roleDbContext = new Mock<AuthenticationOrganizationContextTest>();

            usuarioDbContext.Setup(udc => udc.Usuarios).ReturnsDbSet(usuarios);
            roleDbContext.Setup(udc => udc.Roles).ReturnsDbSet(roles);

            //criação dos repositórios
            var usuarioRepositorio = new UsuarioRepositoryTest(usuarioDbContext.Object,
                new Mock<ILogger<UsuarioRepository>>().Object);


            var roleRepositorio = new RoleRepositoryTest(roleDbContext.Object,
                new Mock<ILogger<RoleRepository>>().Object);

            var roleJwtClaimRepositorio = new RoleJwtClaimRepositoryTest(roleDbContext.Object,
                new Mock<ILogger<RoleJwtClaimRepository>>().Object);


            return new UsuarioService(usuarioRepositorio, roleRepositorio, roleJwtClaimRepositorio);
        }

        public static RoleService GetRoleService(IList<Role> roles)
        {
            var roleDbContext = new Mock<AuthenticationOrganizationContextTest>();
            roleDbContext.Setup(udc => udc.Roles).ReturnsDbSet(roles);

            var roleRepositorio = new RoleRepositoryTest(roleDbContext.Object,
                    new Mock<ILogger<RoleRepository>>().Object);

            return new RoleService(roleRepositorio);
        }
    }
}
