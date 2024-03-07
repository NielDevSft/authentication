namespace JWTAuthentication.Domain.Authentications.Jwts
{
    public class JwtAuthResult()
    {
        public string AccessToken { get; init; }
        public RefreshToken RefreshToken { get; init; }
    }
}
