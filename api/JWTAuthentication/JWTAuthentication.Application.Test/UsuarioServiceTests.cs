using AutoFixture;
using JWTAuthentication.Application.Services;
using JWTAuthentication.Application.Test.Factory;
using JWTAuthentication.Application.Test.Utils;
using JWTAuthentication.Domain.Usuarios;

namespace JWTAuthentication.Application.Test
{
    public class UsuarioServiceTests
    {
        private static readonly Fixture fixture = new Fixture();

        [Fact]
        public async void Create_ValidInput_ShouldCreate()
        {
            var usuarios = GenerateFakeData.Usuarios(fixture, 4);
            var roles = GenerateFakeData.Roles(fixture, 4);

            UsuarioService usuarioService = ServiceFactory.GetUsuarioService(usuarios, roles);

            // prepare
            var usuario = new Usuario()
            {
                Username = "Cezinha",
                PasswordHash = HashCreator.Hash("admin123123"),
                Email = "cezinha@vaivendo.com.br",
            };
            // execute
            var usuarioCreated = await usuarioService.Create(usuario);
            //verify
            Assert.True(usuarioCreated is { Removed: false, Active: true });
        }

        [Fact]
        public async void Create_InvalidInput_ShouldThrow()
        {
            var usuarios = GenerateFakeData.Usuarios(fixture, 4);
            var roles = GenerateFakeData.Roles(fixture, 4);
            UsuarioService usuarioService = ServiceFactory.GetUsuarioService(usuarios, roles);
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
            var usuarios = GenerateFakeData.Usuarios(fixture, 4);
            var roles = GenerateFakeData.Roles(fixture, 4);
            UsuarioService usuarioService = ServiceFactory.GetUsuarioService(usuarios, roles);

            var usuariosObtidos = await usuarioService.GetAll();

            Assert.Equal(usuarios.Count, usuariosObtidos.Count);
        }
        [Fact]
        public async void GetById_OutRangeId_ShouldThrows()
        {
            //prepare
            var usuarios = GenerateFakeData.Usuarios(fixture, 1);
            var roles = GenerateFakeData.Roles(fixture, 1);
            UsuarioService usuarioService = ServiceFactory.GetUsuarioService(usuarios, roles);
            //execute
            var error = await Assert.ThrowsAsync<Exception>(() => usuarioService.GetById(5));
            //asserts
            Assert.Equal("Item não encontrado", error.Message);
        }
        [Fact]
        public async void Update_ValidInput_RetrunNewUpdateAt()
        {
            //prepare
            var usuarios = GenerateFakeData.Usuarios(fixture, 1);
            var roles = GenerateFakeData.Roles(fixture, 1);
            UsuarioService usuarioService = ServiceFactory.GetUsuarioService(usuarios, roles);
            var usuario = usuarios.First();
            var prenultimaAtualizacao = usuario.UpdateAt;
            usuario.Username = "Usuário atualizado";

            //execute
            var usuarioAtualizado = await usuarioService.Update(usuario.Id, usuario);

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
            UsuarioService usuarioService = ServiceFactory.GetUsuarioService(usuarios, roles);
            var usuario = usuarios.First();

            //execute
            await usuarioService.Delete(usuario.Id);

            var usuariosExisting = (await usuarioService.GetAll());

            //asserts
            Assert.True(usuariosExisting.Count().Equals(0));
        }
        [Fact]
        public async void Delete_InvalidInput_ShouldThrows()
        {
            //prepare
            var usuarios = GenerateFakeData.Usuarios(fixture, 1);
            var roles = GenerateFakeData.Roles(fixture, 1);
            UsuarioService usuarioService = ServiceFactory.GetUsuarioService(usuarios, roles);

            //execute
            var error = await Assert.ThrowsAsync<Exception>(() => usuarioService.Delete(-5));

            //asserts
            Assert.Equal("Item não encontrado", error.Message);
        }
        [Fact]
        public async void SetRoleList_ValidInput_ReturnsUsuarioWithRoles()
        {
            //prepare
            var usuarios = GenerateFakeData.Usuarios(fixture, 1);
            var roles = GenerateFakeData.Roles(fixture, 3);
            UsuarioService usuarioService = ServiceFactory.GetUsuarioService(usuarios, roles);

            var rolesListOnlyIds = roles.Select(r => r.Id).ToList();

            //execute
            var usuarioWithRoles = await usuarioService.SetRoleList(usuarios.First().Id, rolesListOnlyIds);

            //assertis
            Assert.Equal(3, usuarioWithRoles.JwtClaims.Subject.Split("|").Count());
            
            Assert.Contains(usuarioWithRoles.JwtClaims.Subject.Split("|").First().Trim(), roles.Select(r => r.Name));
            Assert.Equal(usuarioWithRoles.JwtClaims.RoleJwtClaims.Count, roles.Count);
        }
    }
}