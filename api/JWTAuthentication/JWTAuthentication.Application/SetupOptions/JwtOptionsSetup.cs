using JWTAuthentication.Application.Authentications;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace JWTAuthentication.Application.SetupOptions
{
    public class JwtOptionsSetup : IConfigureOptions<JwtOptions>
    {
        private readonly IConfiguration _configuration;
        public JwtOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void Configure(JwtOptions options)
        {
            _configuration.GetSection("JwtConfig").Bind(options);
        }
    }
}
