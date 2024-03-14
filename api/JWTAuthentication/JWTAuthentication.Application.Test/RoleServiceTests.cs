//using AutoFixture;
//using JWTAuthentication.Application.Services;
//using JWTAuthentication.Application.Test.Factory;
//using JWTAuthentication.Application.Test.Utils;
//using JWTAuthentication.Domain.Usuarios.Roles;

//namespace JWTAuthentication.Application.Test
//{
//    public class RoleServiceTests
//    {
//        private static readonly Fixture fixture = new Fixture();

//        [Fact]
//        public async void Create_ValidInput_ShouldCreate()
//        {
//            var roles = GenerateFakeData.Roles(fixture, 4);
//            RoleService roleService = ServiceFactory.GetRoleService(roles);

//            // prepare
//            var role = new Role()
//            {
//                Name = "Admin",
//            };
//            // execute
//            var roleCreated = await roleService.Create(role);
//            // asserts
//            Assert.True(roleCreated is { Removed: false, Active: true, Name: "ADMIN" });
//        }
//        [Fact]
//        public async void Create_InvalidInput_ShouldThrow()
//        {
//            var roles = GenerateFakeData.Roles(fixture, 4);
//            RoleService roleService = ServiceFactory.GetRoleService(roles);
//            // prepare
//            var role = new Role()
//            {
//                Name = "Ad",
//            };
//            // execure
//            var entityErros = await Assert.ThrowsAsync<Exception>(() => roleService.Create(role));
//            //verify
//            Assert.Contains("Nome de role inválido", entityErros.Message);
//        }
//        [Fact]
//        public async void GetAll_NoParams_ReturnAll()
//        {
//            var roles = GenerateFakeData.Roles(fixture, 4);
//            RoleService roleService = ServiceFactory.GetRoleService(roles);

//            var usuariosObtidos = await roleService.GetAll();

//            Assert.Equal(roles.Count, usuariosObtidos.Count);
//        }
//        [Fact]
//        public async void Delete_ValidInput_ShouldDelete()
//        {
//            //prepare
//            var roles = GenerateFakeData.Roles(fixture, 1);
//            RoleService roleService = ServiceFactory.GetRoleService(roles);
//            var role = roles.First();

//            //execute
//            await roleService.Delete(role.Id);

//            var usuariosExisting = (await roleService.GetAll());

//            //asserts
//            Assert.True(usuariosExisting.Count().Equals(0));
//        }
//        [Fact]
//        public async void GetById_OutRangeId_ShouldThrows()
//        {
//            //prepare
//            var usuarios = GenerateFakeData.Usuarios(fixture, 1);
//            var roles = GenerateFakeData.Roles(fixture, 1);
//            RoleService roleService = ServiceFactory.GetRoleService(roles);
//            //execute
//            var error = await Assert.ThrowsAsync<Exception>(() => roleService.GetById(5));
//            //asserts
//            Assert.Equal("Item não encontrado", error.Message);
//        }
//        [Fact]
//        public async void Delete_InvalidInput_ShouldThrows()
//        {
//            //prepare
//            var roles = GenerateFakeData.Roles(fixture, 1);
//            RoleService roleService = ServiceFactory.GetRoleService(roles);

//            //execute
//            var error = await Assert.ThrowsAsync<Exception>(() => roleService.Delete(-5));

//            //asserts
//            Assert.Equal("Item não encontrado", error.Message);
//        }
//    }
//}
