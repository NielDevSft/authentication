using JWTAuthentication.Application.Test.Contexts;
using JWTAuthentication.Domain.Usuarios;
using JWTAuthentication.Persistence.Abstractions;
using JWTAuthentication.Persistence.Repositories;
using Microsoft.Extensions.Logging;

namespace JWTAuthentication.Application.Test.Repositories
{
    public class UsuarioRepositoryTest : UsuarioRepository
    {
        public UsuarioRepositoryTest(AuthenticationOrganizationContextTest context, ILogger<Repository<Usuario>> logger) : base(context, logger)
        {
            DbSet = context.Usuarios;
        }
    }
}
