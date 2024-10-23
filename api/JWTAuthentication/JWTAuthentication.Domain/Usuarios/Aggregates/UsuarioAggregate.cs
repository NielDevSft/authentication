using JWTAuthentication.Domain.Core.Aggregates;
using JWTAuthentication.Domain.Usuarios.Events;
using JWTAuthentication.Domain.Usuarios.Service;
using JWTAuthentication.Domain.Usuarios.States;

namespace JWTAuthentication.Domain.Usuarios.Aggregates
{
    public class UsuarioAggregate : AggregateRoot<UsuarioAggregate, UsuarioId>
    {
        private readonly UsuarioState _state;
        public UsuarioAggregate(UsuarioId id, IUsuarioService usuarioService) : base(id)
        {
            UsuarioState state = new UsuarioState(usuarioService);
            _state = state;
            Register(_state);
        }

        public void Create(Usuario usuario)
        {
            Emit(new CreateUsuarioEvent(usuario));
        }
    }
}
