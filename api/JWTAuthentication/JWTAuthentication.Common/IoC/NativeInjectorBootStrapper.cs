using JWTAuthentication.Application.Authentications;
using JWTAuthentication.Application.Configurations;
using JWTAuthentication.Application.SetupOptions;
using JWTAuthentication.Persistence.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Threading;


namespace JWTAuthentication.Common.IoC
{
    public class NativeInjectorBootStrapper()
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            MvcConfiguration.AddMvcSecurity(services);
            //jwt
            var jwtTokenConfig = configuration.GetSection("JwtConfig").Get<JwtOptions>()!;

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer();
            services.ConfigureOptions<JwtOptionsSetup>();
            services.ConfigureOptions<JwtBeareOptionsSetup>();


            // Repositories
            InjectorRepositories.AddRepositories(services);

            //Services Bussines
            InjectorServices.AddServices(services, configuration);

            services.AddDbContext<AuthenticationOrganizationContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                    npgsql =>
                    {
                        npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "app");
                        npgsql.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), errorCodesToAdd: null);
                    });
                options.EnableSensitiveDataLogging();
            });
        }
        public static void RegisterApplication(IApplicationBuilder application)
        {
            using (var serviceScope = application.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<AuthenticationOrganizationContext>();
                var cultureInfo = new CultureInfo("pt");
                CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
                CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

                var retries = 10;
                while (retries > 0)
                {
                    try
                    {
                        context.Database.Migrate();
                        break;
                    }
                    catch (NpgsqlException ex)
                    {
                        retries--;
                        Console.WriteLine($"Migration falhou, tentativas restantes: {retries}. Erro: {ex.Message}");
                        if (retries == 0) throw;
                        Thread.Sleep(3000);
                    }
                }
            }
        }

    }
}
