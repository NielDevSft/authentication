//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.DependencyInjection;
//namespace EmpresaAPI.Configurations
//{
//    public static class SwaggerConfiguration
//    {
//        public static void AddSwagger(this IServiceCollection services)
//        {
//            services.AddSwaggerGen(c =>
//            {
//                c.SwaggerDoc("v1", new OpenApiInfo { Title = "JWT Auth Demo", Version = "v1" });
//                var securityScheme = new OpenApiSecurityScheme
//                {
//                    Name = "JWT Authentication",
//                    Description = "Enter JWT Bearer token **_only_**",
//                    In = ParameterLocation.Header,
//                    Type = SecuritySchemeType.Http,
//                    Scheme = "bearer", // must be lowercase
//                    BearerFormat = "JWT",
//                    Reference = new OpenApiReference
//                    {
//                        Id = JwtBearerDefaults.AuthenticationScheme,
//                        Type = ReferenceType.SecurityScheme
//                    }
//                };
//                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
//                c.AddSecurityRequirement(new OpenApiSecurityRequirement
//            {
//                {securityScheme, Array.Empty<string>()}
//            });
//            });

//        }
//    }
//}
