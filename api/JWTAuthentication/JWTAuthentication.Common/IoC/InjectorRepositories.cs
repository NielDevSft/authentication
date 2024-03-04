using JWTAuthentication.Domain.Usuarios.JwsClaims.Roles.UsuariosRole.Repository;
using JWTAuthentication.Domain.Usuarios.Repository;
using JWTAuthentication.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace JWTAuthentication.Common.IoC
{
    public class InjectorRepositories
    {
        public static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IRoleJwtClaimRepository, UsuarioRoleRepository>();
            //services.AddScoped<IItemEstoqueRepository, ItemEstoqueRepository>();
            //services.AddScoped<IPedidoRepository, PedidoRepository>();
            //services.AddScoped<IItemPedidoRepository, ItemPedidoRepository>();
        }
    }
}
