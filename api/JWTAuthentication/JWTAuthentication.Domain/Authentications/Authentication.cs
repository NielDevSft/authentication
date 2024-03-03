using FluentValidation;
using JWTAuthentication.Domain.Core.Models;

namespace JWTAuthentication.Domain.Authentications
{
    public class Authentication : Entity<Authentication>
    {
        public Authentication()
        {
            RuleFor(a => a.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("O e-mail não é válido")
                .WithErrorCode("400");
        }
        public string Email { get; set; }
        public string Password { get; set; }
        public IEnumerable<string[]> Roles { get; set; }

        public override bool IsValid()
        {
            var validatorResult = Validate(this);
            ValidationResult = validatorResult;
            return ValidationResult.IsValid;
        }
    }
}
