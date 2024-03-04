using JWTAuthentication.Application.SetupOptions;
using JWTAuthentication.Persistence.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;


namespace JWTAuthentication.Common.IoC
{
    public class NativeInjectorBootStrapper()
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            //smore about configuring Swagger/ OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();

            //jwt
            services.AddAuthentication();
            services.ConfigureOptions<JwtOptionsSetup>();
            services.ConfigureOptions<JwtBeareOptionsSetup>();



            // Repositories
            InjectorRepositories.AddRepositories(services);

            //Services Bussines
            InjectorServices.AddServices(services);

            services.AddDbContext<AuthenticationOrganizationContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                options.EnableSensitiveDataLogging(); // Isso permite que dados sensíveis também sejam logados
            });
        }
        public static void RegisterApplication(IApplicationBuilder application)
        {
            application.UseAuthentication();

            using (var serviceScope = application.ApplicationServices.CreateScope())
            {
                try
                {
                    var cultureInfo = new CultureInfo("pt"); // Substitua "en-US" pela cultura desejada
                    CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
                    CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
                    var context = serviceScope.ServiceProvider.GetRequiredService<AuthenticationOrganizationContext>();
                    context.Database.Migrate();
                }
                catch (SqlException ex)
                {
                    if (ex.ErrorCode == -2146232060)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            


        }

    }
}
