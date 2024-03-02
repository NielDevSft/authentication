using JWTAuthentication.Persistence.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace JWTAuthentication.Common.IoC
{
    public class NativeInjectorBootStrapper()
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            //jwt
            services.AddAuthentication();
            services.ConfigureOptions<JwtOptions>();
            services.ConfigureOptions<JwtBearerOptions>();



            // Repositories
            InjectorRepositories.AddRepositories(services);

            //Services Bussines
            InjectorServices.AddServices(services);

            //services.AddDbContext<EmpresaOrganizationContext>(options =>
            //{
            //    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            //    options.EnableSensitiveDataLogging(); // Isso permite que dados sensíveis também sejam logados
            //});
        }
        public static void ApplicationUses(IApplicationBuilder application)
        {
            application.UseAuthentication();
        }

    }
}
