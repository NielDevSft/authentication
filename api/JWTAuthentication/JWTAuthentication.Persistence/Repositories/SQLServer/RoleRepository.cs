using JWTAuthentication.Domain.Usuarios.Roles;
using JWTAuthentication.Domain.Usuarios.Roles.Repository;
using JWTAuthentication.Persistence.Abstractions;
using JWTAuthentication.Persistence.Contexts;
using Microsoft.Extensions.Logging;

namespace JWTAuthentication.Persistence.Repositories.SQLServer
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        private readonly ILogger<Repository<Role>> _logger;

        public RoleRepository(AuthenticationOrganizationContextSqlServer context, ILogger<RoleRepository> logger) : base(context, logger)
        {
            _logger = logger;
        }

        public async Task<ICollection<Role>> GetManyAllRolesByIdAsync(ICollection<Guid> ids)
        {
            var list = new List<Role>();
            try
            {
                _logger.LogInformation("Buscando Roles");
                list.AddRange(FindAllWhere(r => ids.Contains(r.Uuid)).ToList());
                _logger.LogInformation("Finalidado");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return list;
            }
            return list;
        }
    }
}
