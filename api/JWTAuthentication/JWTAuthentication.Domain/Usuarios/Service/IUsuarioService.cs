using JWTAuthentication.Domain.Core.Interfaces;

namespace JWTAuthentication.Domain.Usuarios.Service
{
    public interface IUsuarioService : IService<Usuario>
    {
        Task<bool> IsAnExistingUser(string email);
        Task<bool> IsValidUserCredentials(string email, string password);
        Task<int> GetUserId(string email);
        Task<string[]> GetUserRole(string email);
    }

}
