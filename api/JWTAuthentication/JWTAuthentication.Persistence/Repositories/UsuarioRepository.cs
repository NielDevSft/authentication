using JWTAuthentication.Domain.Usuarios;
using JWTAuthentication.Domain.Usuarios.Repository;
using JWTAuthentication.Persistence.Abstractions;
using JWTAuthentication.Persistence.Contexts;

namespace JWTAuthentication.Persistence.Repositories
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(AuthenticationOrganizationContext context) : base(context)
        {
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
