namespace JWTAuthentication.Domain.Authentications.Jwts
{
    public class RefreshToken()
    {
        public string UserName { get; init; }
        public string TokenString { get; init; }
        public DateTime ExpireAt { get; init; }
    }
}
