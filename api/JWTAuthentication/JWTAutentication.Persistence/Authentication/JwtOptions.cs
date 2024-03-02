namespace JWTAuthentication.Persistence.Authentication
{
    public class JwtOptions
    {
        public IEnumerable<string> Issuer { get; init; }
        public string Audience { get; init; }
        public string SecretKey { get; init; }
    }
}
