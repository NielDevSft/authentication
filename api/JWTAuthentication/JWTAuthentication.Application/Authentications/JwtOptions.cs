using Microsoft.Extensions.Options;

namespace JWTAuthentication.Application.Authentications
{
    public class JwtOptions : IOptions<JwtOptions>
    {
        public string Issuer { get; init; }
        public string Audience { get; init; }
        public string SecretKey { get; init; }

        public JwtOptions Value => this;
    }
}
