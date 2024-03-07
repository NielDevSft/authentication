using JWTAuthentication.Domain.Authentications.Jwts;

namespace JWTAuthentication.Domain.Authentications.Services
{
    public interface IAuthenticationJwtService
    {
        public Task<JwtAuthResult> Login(Authentication authentication);
        public Task Logout(string userName);
        public Task<JwtAuthResult> RefreshToken(string refreshToken, string accessToken);
    }
}
