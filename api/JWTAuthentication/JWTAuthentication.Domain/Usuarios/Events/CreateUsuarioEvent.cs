using JWTAuthentication.Domain.Core.Aggregates;
using JWTAuthentication.Domain.Usuarios.Aggregates;

namespace JWTAuthentication.Domain.Usuarios.Events
{
    public class CreateUsuarioEvent: AggregateEvent<UsuarioAggregate, UsuarioId>
    {
        public CreateUsuarioEvent(Usuario usuario)
        {
            Usuario = usuario;
        }
        public Usuario Usuario { get; }
    }
}
