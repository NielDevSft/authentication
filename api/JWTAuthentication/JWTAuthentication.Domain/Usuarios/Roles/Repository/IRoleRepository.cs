using JWTAuthentication.Domain.Core.Interfaces;

namespace JWTAuthentication.Domain.Usuarios.Roles.Repository
{
    public interface IRoleRepository : IRepository<Role>
    {
        public Task<ICollection<Role>> GetManyAllRolesByIdAsync(ICollection<int> ids);
    }
}
