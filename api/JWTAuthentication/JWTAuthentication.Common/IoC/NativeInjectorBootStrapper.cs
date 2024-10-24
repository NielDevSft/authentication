using JWTAuthentication.Application.Authentications;
using JWTAuthentication.Application.Configurations;
using JWTAuthentication.Application.SetupOptions;
using JWTAuthentication.Common.IoC.Repository;
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
            string? projectName = configuration.GetSection("ProjectInfo")
                                              .GetValue<string>("ProjectName");
            switch (projectName)
            {
                case ("JWTAuthentication.APICQRS"):
                    InjectorRepositoriesPostgreSQL.AddRepositories(services);
                    break;
                case ("JWTAuthentication.API"):
                    InjectorRepositoriesSQLServer.AddRepositories(services);
                    break;
            }

            //Services Bussines
            InjectorServices.AddServices(services, configuration);

        }
        public static void RegisterApplication(IApplicationBuilder application)
        {
            application.UseAuthentication();
            application.UseAuthorization();
            application.UseRouting();
            application.UseHttpsRedirection();
        }

    }
}
