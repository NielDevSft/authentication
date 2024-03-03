using JWTAuthentication.Domain.Core.Interfaces;

namespace JWTAuthentication.Domain.Usuarios.Repository
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        bool TryGetValue(string username, string password);
        bool TryGetValue(string username);
    }
}
