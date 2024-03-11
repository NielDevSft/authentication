using JWTAuthentication.Domain.Usuarios;
using JWTAuthentication.Domain.Usuarios.Roles;
using JWTAuthentication.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace JWTAuthentication.Application.Test.Contexts
{
    public class AuthenticationOrganizationContextTest : AuthenticationOrganizationContext
    {
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
    }
}
