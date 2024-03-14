using AutoFixture;
using JWTAuthentication.Application.Test.Contexts;
using JWTAuthentication.Application.Test.Factory;
using JWTAuthentication.Application.Test.Utils;
using JWTAuthentication.Domain.Authentications.Jwts;
using JWTAuthentication.Domain.Authentications.Services;
using JWTAuthentication.Domain.Usuarios;
using JWTAuthentication.Domain.Usuarios.Roles;
using JWTAuthentication.Domain.Usuarios.Service;
using Moq;

namespace JWTAuthentication.Application.Test
{
    public class AuthenticationJwtServiceTests
    {
        private static readonly Fixture fixture = new Fixture();
        private static readonly Mock<AuthenticationOrganizationContextTest> dbContext =
            new Mock<AuthenticationOrganizationContextTest>();

        [Fact]
        public async void Login_ValidInput_ShouldReturn()
        {
            var usuarios = GenerateFakeData.Usuarios(fixture, 4);
            var roles = GenerateFakeData.Roles(fixture, 4);
            var roleJwtClaims = GenerateFakeData.RoleJwtClaims(fixture, 4);
            usuarios[1].PasswordHash = HashCreator.Hash("admin123123");
            usuarios[1].Email = "teste-email@gmail.com";
            usuarios[1].JwtClaimId = 3;

            JwtAuthResult jwtAuthResult = null;
            using (AuthenticationJwtFactory factory = new AuthenticationJwtFactory(dbContext))
            {
                var data = new Dictionary<string, IList<object>>();
                data["usuarios"] = usuarios.ToArray();
                data["roleJwtClaims"] = roleJwtClaims.ToArray();
                data["roles"] = roles.ToArray();
                IAuthenticationJwtService usuarioService =
                    factory.GetServiceInstace(data);

                usuarios[1] = await PrepareUsuarioToLogin(usuarios[1], roles);

                jwtAuthResult = await usuarioService.Login(new()
                {
                    Email = "teste-email@gmail.com",
                    Password = HashCreator.Hash("admin123123")
                });
            }
            //foi considerado um sucesso pela limitação do mock para fazer includes
            Assert.NotEmpty(jwtAuthResult.AccessToken);
            Assert.True(jwtAuthResult.RefreshToken.UserName == usuarios[1].Username);
            Assert.Equal(543, jwtAuthResult.AccessToken.Length);
        }
        [Fact]
        public async void RefreshToken_ValidInput_ShouldReturn()
        {
            var usuarios = GenerateFakeData.Usuarios(fixture, 4);
            var roles = GenerateFakeData.Roles(fixture, 4);
            var roleJwtClaims = GenerateFakeData.RoleJwtClaims(fixture, 4);

            usuarios[1].Email = "email@valido.com";
            usuarios[1].PasswordHash = HashCreator.Hash("admin123123");

            JwtAuthResult jwtAuthResultComRefreshToken1 = null;
            JwtAuthResult jwtAuthResultComRefreshToken2 = null;
            using (AuthenticationJwtFactory factory = new AuthenticationJwtFactory(dbContext))
            {
                var data = new Dictionary<string, IList<object>>();
                data["usuarios"] = usuarios.ToArray();
                data["roleJwtClaims"] = roleJwtClaims.ToArray();
                data["roles"] = roles.ToArray();
                
                IAuthenticationJwtService usuarioService =
                    factory.GetServiceInstace(data);

                usuarios[1].Email = "email@valido.com";
                usuarios[1].PasswordHash = HashCreator.Hash("admin123123");
                usuarios[1] = await PrepareUsuarioToLogin(usuarios[1], roles);

                jwtAuthResultComRefreshToken1 = await usuarioService.Login(new()
                {
                    Email = usuarios[1].Email,
                    Password = HashCreator.Hash("admin123123")
                });

                jwtAuthResultComRefreshToken2 = await usuarioService
                    .RefreshToken(jwtAuthResultComRefreshToken1.RefreshToken.TokenString,
                        jwtAuthResultComRefreshToken1.AccessToken);
            }

            Assert.False(jwtAuthResultComRefreshToken1.RefreshToken.TokenString ==
                jwtAuthResultComRefreshToken2.RefreshToken.TokenString);
        }

        public async Task<Usuario> PrepareUsuarioToLogin(Usuario usuario, IList<Role> roles)
        {
            UsuarioFactory factory = new UsuarioFactory(dbContext);
            IUsuarioService usuarioService = factory.GetServiceInstace();
            return await usuarioService
                .SetRoleList(usuario.Id, roles.Select(r => r.Id).ToList());
        }

    }
}
