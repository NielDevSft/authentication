using JWTAuthentication.Domain.Authentications.Jwts;

namespace JWTAuthentication.API.Dtos.Logins
{
    public record JwtAuthResultDto(string AccessToken, RefreshToken RefreshToken)
    {
    }
}
