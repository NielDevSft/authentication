using FluentValidation;
using JWTAuthentication.Domain.Core.Models;
using JWTAuthentication.Domain.Usuarios.JwsClaims;

namespace JWTAuthentication.Domain.Usuarios
{
    public class Usuario : Entity<Usuario>
    {
        public Usuario()
        {
            RuleFor(u => u.Email)
                .EmailAddress()
                .WithMessage("E-mail inválido.");
            RuleFor(u => u.PasswordHash)
                .Length(40)
                .WithMessage("Senha em formato inválido.");
        }
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Username { get; set; } = null!;
        public JwtClaim JwtClaims { get; set; }
        public int? JwtClaimId { get; set; }
        public override bool IsValid()
        {
            var validatorResult = Validate(this);
            ValidationResult = validatorResult;
            return ValidationResult.IsValid;
        }
    }
}
