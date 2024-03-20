using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

namespace JWTAuthentication.Application.Configurations
{
    public static class LoggingConfiguration
    {
        public static Serilog.Core.Logger GetConfiguration(IConfigurationBuilder config)
        {
            var settings = config.Build();
            var enviroment = settings["ENVIRONMENT"];

            return new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(settings.ConfigureElasticSink(enviroment))
                .Enrich.WithProperty("Environment", enviroment)
                .ReadFrom.Configuration(settings)
                .CreateLogger();
        }

        private static ElasticsearchSinkOptions ConfigureElasticSink(this IConfiguration config, string enviroment)
        {
            var uri = new Uri(config["ElasticConfiguration:Uri"]);
            var format = $"{Assembly.GetExecutingAssembly()
                .GetName()
                .Name!
                .ToLower()
                .Replace('.', '-')}-{enviroment.ToLower()}-" + "{0:yyyy.MM}";
            return new ElasticsearchSinkOptions(uri)
            {
                AutoRegisterTemplate = true,
                AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv8,
                IndexFormat = format,
                NumberOfReplicas = 1,
                NumberOfShards = 2,
            };
        }
    }
}
