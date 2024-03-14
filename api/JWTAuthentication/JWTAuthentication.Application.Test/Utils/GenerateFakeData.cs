using AutoFixture;
using JWTAuthentication.Domain.Usuarios;
using JWTAuthentication.Domain.Usuarios.JwsClaims;
using JWTAuthentication.Domain.Usuarios.Roles;
using JWTAuthentication.Domain.Usuarios.Roles.RoleJwtClaims;

namespace JWTAuthentication.Application.Test.Utils
{
    public static class GenerateFakeData
    {
        public static IList<Role> Roles(Fixture fixture, int numItens)
        {
            var roles = new List<Role>();
            while (numItens > 0)
            {
                roles.Add(fixture.Build<Role>().With(u => u.RoleJwtClaims, []).Create());
                numItens--;
            }
            return roles;
        }
        public static IList<Usuario> Usuarios(Fixture fixture, int numItens)
        {
            var users = new List<Usuario>();
            while (numItens > 0)
            {
                users.Add(fixture.Build<Usuario>().With(u => u.JwtClaims, new JwtClaim()).Create());
                numItens--;
            }
            return users;
        }

        public static IList<RoleJwtClaim> RoleJwtClaims(Fixture fixture, int numItens)
        {

            var roleJwtClaims = new List<RoleJwtClaim>();
            while (numItens > 0)
            {
                roleJwtClaims.Add(fixture.Build<RoleJwtClaim>()
                    .With(u => u.JwtClaim, new JwtClaim())
                    .With(u => u.Role, new Role())
                    .Create());
                numItens--;
            }
            return roleJwtClaims;
        }
    }
}
