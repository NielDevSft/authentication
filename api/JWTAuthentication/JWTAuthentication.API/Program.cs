
using JWTAuthentication.API.Filters;
using JWTAuthentication.Application.Configurations;
using JWTAuthentication.Common.IoC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.OpenApi;
using Serilog;

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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Authentication API", Version = "v1" });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter JWT Bearer token **_only_**",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    };
    c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);
    c.AddSecurityRequirement(_ => new OpenApiSecurityRequirement
    {
        { new OpenApiSecuritySchemeReference(JwtBearerDefaults.AuthenticationScheme), new List<string>() }
    });
    c.OperationFilter<AuthorizeOperationFilter>();
});


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("./swagger/v1/swagger.json", "Authentication API");
    c.RoutePrefix = string.Empty;
    c.ConfigObject.AdditionalItems["persistAuthorization"] = true;
});
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
NativeInjectorBootStrapper.RegisterApplication(app);

app.Run();
