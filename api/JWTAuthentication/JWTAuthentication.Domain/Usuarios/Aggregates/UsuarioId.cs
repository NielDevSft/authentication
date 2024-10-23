using JWTAuthentication.Domain.Core.Core;

namespace JWTAuthentication.Domain.Usuarios.Aggregates
{
    public class UsuarioId : Identity<UsuarioId>

    {
        public UsuarioId(string value) : base(value)
        {
        }
    }
}
