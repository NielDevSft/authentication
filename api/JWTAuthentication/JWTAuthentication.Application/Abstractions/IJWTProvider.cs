using JWTAuthentication.Domain.Authentications.Jwts;
using JWTAuthentication.Domain.Usuarios;
using JWTAuthentication.Domain.Usuarios.Roles;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace JWTAuthentication.Application.Abstractions
{
    public interface IJWTProvider
    {
        //public string GenerateToken(Usuario usuario, ICollection<Role> roles, DateTime now);
        JwtAuthResult GenerateTokens(string username, Claim[] claims, DateTime now);
        IImmutableDictionary<string, RefreshToken> UsersRefreshTokensReadOnlyDictionary { get; }
        JwtAuthResult Refresh(string refreshToken, string accessToken, DateTime now);
        void RemoveExpiredRefreshTokens(DateTime now);
        void RemoveRefreshTokenByUserName(string userName);
        (ClaimsPrincipal, JwtSecurityToken?) DecodeJwtToken(string token);

    }

}
