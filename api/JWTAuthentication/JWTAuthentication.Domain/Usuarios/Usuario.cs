using FluentValidation;
using JWTAuthentication.Domain.Core.Models;
using JWTAuthentication.Domain.Usuarios.JwsClaims;
using System;

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
            JwtClaims = new JwtClaim();
        }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public JwtClaim JwtClaims { get; set; }
        public Guid? JwtClaimUuid { get; set; }
        public override bool IsValid()
        {
            var validatorResult = Validate(this);
            ValidationResult = validatorResult;
            return ValidationResult.IsValid;
        }
    }
}
