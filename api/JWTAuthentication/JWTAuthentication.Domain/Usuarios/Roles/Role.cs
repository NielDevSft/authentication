using FluentValidation;
using JWTAuthentication.Domain.Core.Models;
using JWTAuthentication.Domain.Usuarios.Roles.RoleJwtClaims;

namespace JWTAuthentication.Domain.Usuarios.Roles
{
    public class Role : Entity<Role>
    {
        public Role()
        {
            RuleFor(i => i.Name)
                .NotEmpty()
                .MinimumLength(3)
                .WithMessage("Nome de role inválido");
        }
        public string Name { get; set; } = null!;
        public virtual ICollection<RoleJwtClaim> RoleJwtClaims { get; set; } = new List<RoleJwtClaim>();
        public override bool IsValid()
        {
            return true;
        }
    }
}
