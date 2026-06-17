namespace JWTAuthentication.Domain.Usuarios.Roles.Service
{
    public interface IRoleService
    {
        public Task<Role> Create(Role role, CancellationToken cancellationToken);
        public Task Delete(Guid uuid, CancellationToken cancellationToken);
        public Task<List<Role>> GetAll(CancellationToken cancellationToken);
        public Task<Role> GetById(Guid uuid, CancellationToken cancellationToken);
    }
}
