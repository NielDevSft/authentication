using JWTAuthentication.Domain.Core.Models;
using JWTAuthentication.Domain.Usuarios.JwsClaims;

namespace JWTAuthentication.Domain.Usuarios
{
    public class Usuario : Entity<Usuario>
    {
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Username { get; set; } = null!;
        public JwtClaim JwtClaims { get; set; }
        public int? JwtClaimId { get; set; }
        public override bool IsValid()
        {
            return true;
        }
    }
}
