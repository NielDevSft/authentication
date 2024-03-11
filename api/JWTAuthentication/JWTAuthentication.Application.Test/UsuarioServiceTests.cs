using AutoFixture;
using JWTAuthentication.Application.Services;
using JWTAuthentication.Application.Test.Contexts;
using JWTAuthentication.Application.Test.Repositories;
using JWTAuthentication.Application.Test.Utils;
using JWTAuthentication.Domain.Usuarios;
using JWTAuthentication.Domain.Usuarios.JwsClaims;
using JWTAuthentication.Domain.Usuarios.Roles;
using JWTAuthentication.Persistence.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;


namespace JWTAuthentication.Application.Test
{
    public class UsuarioServiceTests
    {
        private static readonly Fixture Fixture = new Fixture();

        [Fact]
        public async void Create_ValidInput_ShouldCreate()
        {
            var usuarios = GenerateUsuarios();
            var roles = GenerateRoles();

            var initalCount = usuarios.Count();
            UsuarioService usuarioService = UsuarioServiceFactory(usuarios, roles);

            // prepare
            var usuario = new Usuario()
            {
                Username = "Cezinha",
                PasswordHash = HashCreator.Hash("admin123123"),
                Email = "cezinha@vaivendo.com.br",
            };
            // execure
            var usuarioCreated = await usuarioService.Create(usuario);
            //verify
            Assert.True(usuarioCreated is { Removed: false, Active: true });
        }



        [Fact]
        public async void Create_InvalidEmailAndPassword_ShouldThrow()
        {
            var usuarios = GenerateUsuarios();
            var roles = GenerateRoles();
            UsuarioService usuarioService = UsuarioServiceFactory(usuarios, roles);
            // prepare
            var usuario = new Usuario()
            {
                Username = "Cezinha",
                PasswordHash = "admin123123",
                Email = "cezinhavaivendo",
            };
            // execure
            var entityErros = await Assert.ThrowsAsync<Exception>(() => usuarioService.Create(usuario));
            //verify
            Assert.Contains("Senha em formato inválido.", entityErros.Message);
            Assert.Contains("E-mail inválido.", entityErros.Message);
        }
        [Fact]
        public async void GetAll_NoParams_ReturnAll()
        {
            var usuarios = GenerateUsuarios();
            var roles = GenerateRoles();
            UsuarioService usuarioService = UsuarioServiceFactory(usuarios, roles);

            var usuariosObtidos = await usuarioService.GetAll();

            Assert.Equal(usuarios.Count, usuariosObtidos.Count);
        }

        private static UsuarioService UsuarioServiceFactory(IList<Usuario> usuarios, IList<Role> roles)
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

            return new UsuarioService(usuarioRepositorio, roleRepositorio);
        }
        private static IList<Usuario> GenerateUsuarios()
        {

            var users = new List<Usuario>
            {
                Fixture.Build<Usuario>().With(u => u.JwtClaims , new JwtClaim()).Create(),
                Fixture.Build<Usuario>().With(u => u.JwtClaims , new JwtClaim()).Create(),
                Fixture.Build<Usuario>().With(u => u.JwtClaims , new JwtClaim()).Create(),
                Fixture.Build<Usuario>().With(u => u.JwtClaims , new JwtClaim()).Create(),
            };

            return users;
        }
        private static IList<Role> GenerateRoles()
        {

            var roles = new List<Role>
            {
                Fixture.Build<Role>().With(u => u.RoleJwtClaims , []).Create(),
                Fixture.Build<Role>().With(u => u.RoleJwtClaims , []).Create(),

            };

            return roles;
        }
    }
}