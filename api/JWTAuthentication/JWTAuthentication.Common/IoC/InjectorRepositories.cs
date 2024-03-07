using JWTAuthentication.Domain.Usuarios.JwsClaims.Repository;
using JWTAuthentication.Domain.Usuarios.Repository;
using JWTAuthentication.Domain.Usuarios.Roles.Repository;
using JWTAuthentication.Domain.Usuarios.Roles.RoleJwtClaims.Repository;
using JWTAuthentication.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace JWTAuthentication.Common.IoC
{
    public class InjectorRepositories
    {
        public static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IRoleJwtClaimRepository, RoleJwtClaimRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IJwtClaimRepository, JwtClaimRepository>();
            //services.AddScoped<IPedidoRepository, PedidoRepository>();
            //services.AddScoped<IItemPedidoRepository, ItemPedidoRepository>();
        }
    }
}
