namespace JWTAuthentication.Domain.Usuarios.Roles.Service
{
    public interface IRoleService
    {
        public Task<Role> Create(Role role);
        public Task Delete(int id);
        public Task<List<Role>> GetAll();
        public Task<Role> GetById(int id);
    }
}
