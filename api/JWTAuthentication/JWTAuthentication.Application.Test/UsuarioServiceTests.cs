using AutoFixture;
using JWTAuthentication.Application.Test.Contexts;
using JWTAuthentication.Application.Test.Factory;
using JWTAuthentication.Application.Test.Utils;
using JWTAuthentication.Domain.Usuarios;
using JWTAuthentication.Domain.Usuarios.Service;
using Moq;

namespace JWTAuthentication.Application.Test
{
    public class UsuarioServiceTests
    {
        private static readonly Fixture fixture = new Fixture();
        private static readonly Mock<AuthenticationOrganizationContextTest> dbContext =
            new Mock<AuthenticationOrganizationContextTest>();

        [Fact]
        public async void Create_ValidInput_ShouldCreate()
        {

            var usuarios = GenerateFakeData.Usuarios(fixture, 4);
            var roles = GenerateFakeData.Roles(fixture, 4);
            var roleJwtClaims = GenerateFakeData.RoleJwtClaims(fixture, 4);

            Usuario usuarioCreated = null;

            using (UsuarioFactory usuarioFactory = new UsuarioFactory(dbContext))
            {
                var data = new Dictionary<string, IList<object>>();
                data["usuarios"] = usuarios.ToArray();
                data["roles"] = roles.ToArray();
                data["roleJwtClaims"] = roleJwtClaims.ToArray();
                IUsuarioService usuarioService = usuarioFactory.GetServiceInstace(data);

                // prepare
                var usuario = new Usuario()
                {
                    Username = "Cezinha",
                    PasswordHash = HashCreator.Hash("admin123123"),
                    Email = "cezinha@vaivendo.com.br",
                };
                // execute
                usuarioCreated = await usuarioService.Create(usuario);
            }
            // asserts
            Assert.True(usuarioCreated is { Removed: false, Active: true });
        }

        [Fact]
        public async void Create_InvalidInput_ShouldThrow()
        {
            // prepare
            var usuarios = GenerateFakeData.Usuarios(fixture, 4);
            var roles = GenerateFakeData.Roles(fixture, 4);
            var roleJwtClaims = GenerateFakeData.RoleJwtClaims(fixture, 3);

            // execure
            Exception entityErros = null;
            using (UsuarioFactory usuarioFactory = new UsuarioFactory(dbContext))
            {
                var data = new Dictionary<string, IList<object>>();
                data["usuarios"] = usuarios.ToArray();
                data["roles"] = roles.ToArray();
                data["roleJwtClaims"] = roleJwtClaims.ToArray();
                IUsuarioService usuarioService = usuarioFactory.GetServiceInstace(data);
                var usuario = new Usuario()
                {
                    Username = "Cezinha",
                    PasswordHash = "admin123123",
                    Email = "cezinhavaivendo",
                };
                entityErros = await Assert.ThrowsAsync<Exception>(() => usuarioService.Create(usuario));
            }
            //verify
            Assert.Contains("Senha em formato inválido.", entityErros.Message);
            Assert.Contains("E-mail inválido.", entityErros.Message);
        }
        [Fact]
        public async void GetAll_NoParams_ReturnAll()
        {
            var usuarios = GenerateFakeData.Usuarios(fixture, 4);
            var roles = GenerateFakeData.Roles(fixture, 4);
            var roleJwtClaims = GenerateFakeData.RoleJwtClaims(fixture, 3);

            IList<Usuario> usuariosObtidos = new List<Usuario>();

            using (UsuarioFactory usuarioFactory = new UsuarioFactory(dbContext))
            {
                var data = new Dictionary<string, IList<object>>();
                data["usuarios"] = usuarios.ToArray();
                data["roles"] = roles.ToArray();
                data["roleJwtClaims"] = roleJwtClaims.ToArray();
                IUsuarioService usuarioService = usuarioFactory.GetServiceInstace(data);
                usuariosObtidos = await usuarioService.GetAll();
            }

            Assert.Equal(usuarios.Count, usuariosObtidos.Count);
        }
        [Fact]
        public async void GetById_OutRangeId_ShouldThrows()
        {
            //prepare
            var usuarios = GenerateFakeData.Usuarios(fixture, 1);
            var roles = GenerateFakeData.Roles(fixture, 1);
            var roleJwtClaims = GenerateFakeData.RoleJwtClaims(fixture, 3);

            //execute
            Exception error = null;
            using (UsuarioFactory usuarioFactory = new UsuarioFactory(dbContext))
            {
                var data = new Dictionary<string, IList<object>>();
                data["usuarios"] = usuarios.ToArray();
                data["roles"] = roles.ToArray();
                data["roleJwtClaims"] = roleJwtClaims.ToArray();
                IUsuarioService usuarioService = usuarioFactory.GetServiceInstace(data);
                error = await Assert.ThrowsAsync<Exception>(() => usuarioService.GetById(5));
            }
            //asserts
            Assert.Equal("Item não encontrado", error.Message);
        }
        [Fact]
        public async void Update_ValidInput_RetrunNewUpdateAt()
        {
            //prepare
            var usuarios = GenerateFakeData.Usuarios(fixture, 1);
            var roles = GenerateFakeData.Roles(fixture, 1);
            var roleJwtClaims = GenerateFakeData.RoleJwtClaims(fixture, 3);

            var usuario = usuarios.First();
            var prenultimaAtualizacao = usuario.UpdateAt;
            usuario.Username = "Usuário atualizado";

            //execute
            Usuario usuarioAtualizado = null;
            using (UsuarioFactory usuarioFactory = new UsuarioFactory(dbContext))
            {
                var data = new Dictionary<string, IList<object>>();
                data["usuarios"] = usuarios.ToArray();
                data["roles"] = roles.ToArray();
                data["roleJwtClaims"] = roleJwtClaims.ToArray();
                IUsuarioService usuarioService = usuarioFactory.GetServiceInstace(data);
                usuarioAtualizado = await usuarioService.Update(usuario.Id, usuario);
            }

            //asserts
            Assert.True(usuarioAtualizado is
            { Username: "Usuário atualizado" });
            Assert.NotEqual(prenultimaAtualizacao, usuarioAtualizado.UpdateAt);
            Assert.Equal(usuarioAtualizado.CreateAt, usuario.CreateAt);

        }
        [Fact]
        public async void Delete_ValidInput_ShouldDelete()
        {
            //prepare
            var usuarios = GenerateFakeData.Usuarios(fixture, 1);
            var roles = GenerateFakeData.Roles(fixture, 1);
            var roleJwtClaims = GenerateFakeData.RoleJwtClaims(fixture, 3);

            var usuario = usuarios.First();

            IList<Usuario> usuariosExisting = null;
            //execute
            using (UsuarioFactory usuarioFactory = new UsuarioFactory(dbContext))
            {
                var data = new Dictionary<string, IList<object>>();
                data["usuarios"] = usuarios.ToArray();
                data["roles"] = roles.ToArray();
                data["roleJwtClaims"] = roleJwtClaims.ToArray();
                IUsuarioService usuarioService = usuarioFactory.GetServiceInstace(data);
                await usuarioService.Delete(usuario.Id);
                usuariosExisting = (await usuarioService.GetAll());
            }

            //asserts
            Assert.True(usuariosExisting.Count().Equals(0));
        }
        [Fact]
        public async void Delete_InvalidInput_ShouldThrows()
        {
            //prepare
            var usuarios = GenerateFakeData.Usuarios(fixture, 1);
            var roles = GenerateFakeData.Roles(fixture, 1);
            var roleJwtClaims = GenerateFakeData.RoleJwtClaims(fixture, 3);

            //execute
            Exception error = null;
            using (UsuarioFactory usuarioFactory = new UsuarioFactory(dbContext))
            {
                var data = new Dictionary<string, IList<object>>();
                data["usuarios"] = usuarios.ToArray();
                data["roles"] = roles.ToArray();
                data["roleJwtClaims"] = roleJwtClaims.ToArray();
                IUsuarioService usuarioService = usuarioFactory.GetServiceInstace(data);
                error = await Assert.ThrowsAsync<Exception>(() => usuarioService.Delete(-5));
            }
            //asserts
            Assert.Equal("Item não encontrado", error.Message);
        }
        [Fact]
        public async void SetRoleList_ValidInput_ReturnsUsuarioWithRoles()
        {
            //prepare
            var usuarios = GenerateFakeData.Usuarios(fixture, 1);
            var roles = GenerateFakeData.Roles(fixture, 3);
            var roleJwtClaims = GenerateFakeData.RoleJwtClaims(fixture, 3);

            var rolesListOnlyIds = roles.Select(r => r.Id).ToList();

            Usuario usuarioWithRoles = null;
            //execute
            using (UsuarioFactory usuarioFactory = new UsuarioFactory(dbContext))
            {
                var data = new Dictionary<string, IList<object>>();
                data["usuarios"] = usuarios.ToArray();
                data["roles"] = roles.ToArray();
                data["roleJwtClaims"] = roles.ToArray();
                IUsuarioService usuarioService = usuarioFactory.GetServiceInstace(data);
                usuarioWithRoles = await usuarioService.SetRoleList(usuarios.First().Id, rolesListOnlyIds);

            }
            //assertis
            Assert.Equal(3, usuarioWithRoles.JwtClaims.Subject.Split("|").Count());

            Assert.Contains(usuarioWithRoles.JwtClaims.Subject.Split("|").First().Trim(), roles.Select(r => r.Name));
            Assert.Equal(usuarioWithRoles.JwtClaims.RoleJwtClaims.Count, roles.Count);
        }
    }
}