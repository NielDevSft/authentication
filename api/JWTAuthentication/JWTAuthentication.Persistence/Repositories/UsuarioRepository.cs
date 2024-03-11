using JWTAuthentication.Domain.Usuarios;
using JWTAuthentication.Domain.Usuarios.Repository;
using JWTAuthentication.Persistence.Abstractions;
using JWTAuthentication.Persistence.Contexts;
using Microsoft.Extensions.Logging;

namespace JWTAuthentication.Persistence.Repositories
{
    public class UsuarioRepository
        : Repository<Usuario>, IUsuarioRepository
    {
        private readonly ILogger<Repository<Usuario>> _logger;

        public UsuarioRepository(AuthenticationOrganizationContext context, ILogger<Repository<Usuario>> logger) : base(context, logger)
        {
            _logger = logger;
        }

        public bool TryGetValue(string username, string password)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(string username)
        {
            throw new NotImplementedException();
        }
    }
}
