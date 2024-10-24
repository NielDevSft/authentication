using JWTAuthentication.Domain.Usuarios;
using JWTAuthentication.Domain.Usuarios.Roles;
using JWTAuthentication.Domain.Usuarios.Roles.RoleJwtClaims;
using JWTAuthentication.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace JWTAuthentication.Application.Test.Contexts
{
    public class AuthenticationOrganizationContextTest : AuthenticationOrganizationContextSqlServer
    {
        public AuthenticationOrganizationContextTest(IConfiguration configuration) : base(configuration)
        {
        }

        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<RoleJwtClaim> RoleJwtClaims { get; set; }
    }
}
