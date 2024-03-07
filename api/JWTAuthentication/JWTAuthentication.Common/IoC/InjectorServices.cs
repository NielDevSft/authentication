using EmpresaAPI.Infrastructure;
using JWTAuthentication.Application.Abstractions;
using JWTAuthentication.Application.Authentications;
using JWTAuthentication.Application.Services;
using JWTAuthentication.Application.SetupOptions;
using JWTAuthentication.Domain.Authentications.Services;
using JWTAuthentication.Domain.Usuarios.Roles.Service;
using JWTAuthentication.Domain.Usuarios.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
            //services.AddScoped<IItemEstoqueService, ItemEstoqueService>();
            //services.AddScoped<IPedidoService, PedidoService>();
        }
    }
}
