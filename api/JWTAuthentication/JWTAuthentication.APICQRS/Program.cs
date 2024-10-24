using JWTAuthentication.Application.Configurations;
using JWTAuthentication.Common.IoC;
using JWTAuthentication.Domain.Core;
using JWTAuthentication.Domain.Core.Extensions;
using JWTAuthentication.Domain.Usuarios.Aggregates.Commands;
using JWTAuthentication.Domain.Usuarios.Events;
using JWTAuthentication.Persistence.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Globalization;
using static JWTAuthentication.Domain.Usuarios.Aggregates.Commands.CreateUsuarioCommand;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
EventFlowOptions.New(builder.Services)
.AddCommands([typeof(CreateUsuarioCommand)])
.AddEvents([typeof(CreateUsuarioEvent)])
.AddCommandHandlers([typeof(UsuarioCommandHandler)]);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<AuthenticationOrganizationContextPostgreSQL>();
Log.Logger = LoggingConfiguration.GetConfiguration(builder.Configuration);
builder.Host.UseSerilog();
NativeInjectorBootStrapper.RegisterServices(builder.Services, builder.Configuration);
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Usuario API", Version = "v1" });

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

using (var serviceScope = app.Services.CreateScope())
{
    try
    {
        var cultureInfo = new CultureInfo("pt"); // Substitua "en-US" pela cultura desejada
        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

        var context = serviceScope.ServiceProvider.GetRequiredService<AuthenticationOrganizationContextPostgreSQL>();
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

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("./swagger/v1/swagger.json", "Usuario API");
    c.RoutePrefix = string.Empty;
});


app.UseCors("AllowAll");

app.MapControllers();
NativeInjectorBootStrapper.RegisterApplication(app);
app.Run();
