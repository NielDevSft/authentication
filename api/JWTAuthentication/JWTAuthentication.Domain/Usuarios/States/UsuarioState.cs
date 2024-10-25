using JWTAuthentication.Domain.Core.Aggregates;
using JWTAuthentication.Domain.Usuarios.Aggregates;
using JWTAuthentication.Domain.Usuarios.Events;
using JWTAuthentication.Domain.Usuarios.Service;

namespace JWTAuthentication.Domain.Usuarios.States
{
    public class UsuarioState : AggregateState<UsuarioAggregate, UsuarioId, UsuarioState>,
        IApply<CreateUsuarioEvent>
    {
        public Usuario Usuario { get; set; }

        public void Apply(CreateUsuarioEvent aggregateEvent)
        {
            Usuario = aggregateEvent.Usuario;
        }
    }
}
