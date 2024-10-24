using JWTAuthentication.Application.Abstractions;
using JWTAuthentication.Application.Authentications;
using JWTAuthentication.Application.Services;
using JWTAuthentication.Domain.Authentications.Services;
using JWTAuthentication.Domain.Usuarios.Roles.Service;
using JWTAuthentication.Domain.Usuarios.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JWTAuthentication.Common.IoC
{
    public class InjectorServices
    {
        public static void AddServices(IServiceCollection services, IConfiguration config)
        {

            services.AddHostedService<JwtRefreshTokenCache>();
            services.AddSingleton<IJWTProvider, JwtProvider>();
            services.AddScoped<IAuthenticationJwtService, AuthenticationService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IRoleService, RoleService>();
        }
    }
}
