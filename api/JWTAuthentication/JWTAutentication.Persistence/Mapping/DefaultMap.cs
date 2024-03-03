using JWTAuthentication.Domain.Core.Models;
using JWTAuthentication.Persistence.Extentions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JWTAuthentication.Persistence.Mapping
{
    public class DefaultMap<T> : EntityTypeConfiguration<T> where T : Entity<T>
    {
        [Obsolete]
        public override void Map(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Active)
                .IsRequired();

            builder.Property(e => e.Removed)
                .IsRequired();

            builder.Ignore(e => e.ClassLevelCascadeMode);
            builder.Ignore(e => e.CascadeMode);
            builder.Ignore(e => e.RuleLevelCascadeMode);
        }
    }
}
