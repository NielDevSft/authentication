using JWTAuthentication.Persistence.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace JWTAuthentication.Persistence.Contexts
{
    public class AuthenticationOrganizationContextPostgreSQL : DbContext
    {
        private IConfiguration _configuration;

        public AuthenticationOrganizationContextPostgreSQL(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public AuthenticationOrganizationContextPostgreSQL(DbContextOptions<AuthenticationOrganizationContextPostgreSQL> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ModelBuilderExtensions.AddCommonContextDefinition(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }
    }
}
