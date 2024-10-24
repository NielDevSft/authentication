
using JWTAuthentication.Application.Configurations;
using JWTAuthentication.Common.IoC;
using JWTAuthentication.Persistence.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = LoggingConfiguration.GetConfiguration(builder.Configuration);
builder.Host.UseSerilog();
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MinRequestBodyDataRate = new MinDataRate(100, TimeSpan.FromSeconds(10));
    serverOptions.Limits.MinResponseDataRate = new MinDataRate(100, TimeSpan.FromSeconds(10));
    serverOptions.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2);
    serverOptions.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(1);
});

NativeInjectorBootStrapper.RegisterServices(builder.Services, builder.Configuration);

builder.Services.AddDbContext<AuthenticationOrganizationContextSqlServer>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Authentication API", Version = "v1" });
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authentication API",
        Description = "Enter JWT Bearer token **_only_**",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer", // must be lowercase
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {securityScheme, Array.Empty<string>()}
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
using (var serviceScope = app.Services.CreateScope())
{
    try
    {
        var cultureInfo = new CultureInfo("pt"); // Substitua "en-US" pela cultura desejada
        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        var context = serviceScope.ServiceProvider.GetRequiredService<AuthenticationOrganizationContextSqlServer>();
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

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("./swagger/v1/swagger.json", "Authentication API");
    c.RoutePrefix = string.Empty;
});
app.UseRouting();
app.UseCors("AllowAll");

app.MapControllers();
NativeInjectorBootStrapper.RegisterApplication(app);

app.Run();
