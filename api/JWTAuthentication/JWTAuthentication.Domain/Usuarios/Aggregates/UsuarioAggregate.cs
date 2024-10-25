using JWTAuthentication.Domain.Core.Aggregates;
using JWTAuthentication.Domain.Core.Exceptions;
using JWTAuthentication.Domain.Usuarios.Events;
using JWTAuthentication.Domain.Usuarios.Service;
using JWTAuthentication.Domain.Usuarios.States;

namespace JWTAuthentication.Domain.Usuarios.Aggregates
{
    public class UsuarioAggregate : AggregateRoot<UsuarioAggregate, UsuarioId>
    {
        private readonly IUsuarioService _usuarioService;
        private readonly UsuarioState _state = new UsuarioState();
        public UsuarioAggregate(UsuarioId id, IUsuarioService usuarioService) : base(id)
        {
            _usuarioService = usuarioService;
            Register(_state);
        }

        public void Create(Usuario usuario)
        {
            if (_state.Usuario is not null)
                throw DomainError.With("Usuario already set");
            var usu = _usuarioService.Create(usuario).Result;

            Emit(new CreateUsuarioEvent(new() {
                Uuid = usu.Uuid,
                Email = usu.Email,
                PasswordHash =  usu.PasswordHash,
                Username =  usu.Username,
                CreateAt = usu.CreateAt,
                UpdateAt = usu.UpdateAt,
                JwtClaimUuid = usu.JwtClaimUuid,
                JwtClaims = null,                
            }));
        }
    }
}
