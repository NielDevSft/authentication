using JWTAuthentication.Application.Abstractions;
using JWTAuthentication.Application.Authentications;
using JWTAuthentication.Application.Services;
using JWTAuthentication.Domain.Authentications.Services;
using Microsoft.Extensions.DependencyInjection;

namespace JWTAuthentication.Common.IoC
{
    public class InjectorServices
    {
        public static void AddServices(IServiceCollection services)
        {
            services.AddScoped<IJWTProvider, JwtProvider>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            //services.AddScoped<IItemService, ItemService>();
            //services.AddScoped<IItemEstoqueService, ItemEstoqueService>();
            //services.AddScoped<IPedidoService, PedidoService>();

        }
    }
}
