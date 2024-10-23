using JWTAuthentication.Domain.Core.Aggregates;
using JWTAuthentication.Domain.Usuarios.Aggregates;
using JWTAuthentication.Domain.Usuarios.Events;
using JWTAuthentication.Domain.Usuarios.Service;

namespace JWTAuthentication.Domain.Usuarios.States
{
    public class UsuarioState : AggregateState<UsuarioAggregate, UsuarioId, UsuarioState>,
        IApply<CreateUsuarioEvent>
    {
        private IUsuarioService _usuarioService;
        public Usuario Usuario { get; set; }
        public UsuarioState(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }
        public void Apply(CreateUsuarioEvent aggregateEvent)
        {
            Usuario = _usuarioService.Create(aggregateEvent.Usuario).Result;
        }
    }
}
