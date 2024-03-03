using JWTAuthentication.Domain.Usuarios;
using JWTAuthentication.Domain.Usuarios.JwsClaims;
using JWTAuthentication.Domain.Usuarios.JwsClaims.Roles;
using JWTAuthentication.Domain.Usuarios.JwsClaims.Roles.UsuariosRole;
using JWTAuthentication.Persistence.Extentions;
using JWTAuthentication.Persistence.Mapping;
using Microsoft.EntityFrameworkCore;

namespace JWTAuthentication.Persistence.Contexts
{
    public class AuthenticationOrganizationContext : DbContext
    {
        public AuthenticationOrganizationContext()
        {
        }

        public AuthenticationOrganizationContext(DbContextOptions<AuthenticationOrganizationContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("app");

            modelBuilder.AddConfiguration(new DefaultMap<Usuario>());
            modelBuilder.AddConfiguration(new DefaultMap<Role>());
            modelBuilder.AddConfiguration(new DefaultMap<UsuarioRole>());
            modelBuilder.AddConfiguration(new DefaultMap<JwtClaim>());

            modelBuilder.Entity<Usuario>();
            modelBuilder.Entity<Role>();
            modelBuilder.Entity<UsuarioRole>();
            modelBuilder.Entity<JwtClaim>();


            base.OnModelCreating(modelBuilder);
        }
    }
}
