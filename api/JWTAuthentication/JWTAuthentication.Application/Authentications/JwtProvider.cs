using JWTAuthentication.Application.Abstractions;
using JWTAuthentication.Domain.Usuarios;
using JWTAuthentication.Domain.Usuarios.JwsClaims.Roles;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTAuthentication.Application.Authentications
{
    public class JwtProvider : IJWTProvider
    {
        private readonly JwtOptions _options;

        public JwtProvider(IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }
        public string GenerateToken(Usuario usuario, ICollection<Role> roles)
        {
            var claims = new Claim[] {
                        new Claim(ClaimTypes.Name ,usuario.Username),
                        new Claim(ClaimTypes.Email ,usuario.Email),
                        new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                        new Claim(ClaimTypes.Role, roles.Select(r => r.Name).ToString() ?? string.Empty),
            };
            var signingCredentials = new SigningCredentials
                (new SymmetricSecurityKey(Encoding
                .UTF8
                .GetBytes(_options.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _options.Issuer,
                _options.Audience,
                claims,
                null,
                DateTime.UtcNow.AddHours(1),
                signingCredentials);

            string tokenValue = new JwtSecurityTokenHandler()
                .WriteToken(token);

            return tokenValue;
        }
    }
}
