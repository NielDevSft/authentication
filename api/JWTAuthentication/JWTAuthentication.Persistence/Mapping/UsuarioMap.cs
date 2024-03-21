using JWTAuthentication.Domain.Usuarios;
using JWTAuthentication.Persistence.Extentions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JWTAuthentication.Persistence.Mapping
{
    public class UsuarioMap : EntityTypeConfiguration<Usuario>
    {
        public override void Map(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasIndex(u => u.Email)
            .IsUnique();
        }
    }
}
