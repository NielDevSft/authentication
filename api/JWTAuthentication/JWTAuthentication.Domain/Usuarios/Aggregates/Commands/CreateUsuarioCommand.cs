using JWTAuthentication.Domain.Core.Aggregates;
using JWTAuthentication.Domain.Core.Commands;
using JWTAuthentication.Domain.Core.ReadStores;
using JWTAuthentication.Domain.Usuarios.Events;

namespace JWTAuthentication.Domain.Usuarios.Aggregates.Commands
{
    public class CreateUsuarioCommand : Command<UsuarioAggregate, UsuarioId>
    {
        public CreateUsuarioCommand(
            UsuarioId aggregateId,
            string email,
            string passwordHash,
            string username) : base(aggregateId)
        {
            Email = email;
            PasswordHash = passwordHash;
            Username = username;
        }

        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;

        public class UsuarioCommandHandler : CommandHandler<UsuarioAggregate, UsuarioId, CreateUsuarioCommand>
        {
            public override Task ExecuteAsync(UsuarioAggregate aggregate, CreateUsuarioCommand command, CancellationToken cancellationToken)
            {
                aggregate.Create(new()
                {
                    Email = command.Email,
                    Username = command.Username,
                    PasswordHash = command.PasswordHash
                });
                return Task.CompletedTask;
            }
        }

        public class UsuarioReadModel : IReadModel,
           IAmReadModelFor<UsuarioAggregate, UsuarioId, CreateUsuarioEvent>
        {
            public Usuario Usuario { get; private set; }

            public Task ApplyAsync(
                IReadModelContext context,
                IDomainEvent<UsuarioAggregate, UsuarioId, CreateUsuarioEvent> domainEvent,
                CancellationToken cancellationToken)
            {
                Usuario = domainEvent.AggregateEvent.Usuario;
                return Task.CompletedTask;
            }
        }

    }
}
