using JWTAuthentication.Domain.Usuarios.JwsClaims;
using JWTAuthentication.Domain.Usuarios.Roles.RoleJwtClaims;
using JWTAuthentication.Domain.Usuarios.Roles;
using JWTAuthentication.Domain.Usuarios;
using JWTAuthentication.Persistence.Mapping;
using Microsoft.EntityFrameworkCore;

namespace JWTAuthentication.Persistence.Extentions
{
    public static class ModelBuilderExtensions
    {
        public static void AddConfiguration<TEntity>(this ModelBuilder modelBuilder, EntityTypeConfiguration<TEntity> configuration) where TEntity : class
        {
            configuration.Map(modelBuilder.Entity<TEntity>());
        }
        public static void AddCommonContextDefinition(this ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("app");

            modelBuilder.AddConfiguration(new DefaultMap<Usuario>());
            modelBuilder.AddConfiguration(new DefaultMap<Role>());
            modelBuilder.AddConfiguration(new DefaultMap<RoleJwtClaim>());
            modelBuilder.AddConfiguration(new DefaultMap<JwtClaim>());

            modelBuilder.AddConfiguration(new UsuarioMap());

            modelBuilder.Entity<Usuario>();
            modelBuilder.Entity<Role>();
            modelBuilder.Entity<RoleJwtClaim>();
            modelBuilder.Entity<JwtClaim>();
        }
    }
}