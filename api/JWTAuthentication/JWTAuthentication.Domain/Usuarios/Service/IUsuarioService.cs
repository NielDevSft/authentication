namespace JWTAuthentication.Domain.Usuarios.Service
{
    public interface IUsuarioService
    {
        public Task<Usuario> Create(Usuario usuario);
        public Task Delete(int id);
        public Task<List<Usuario>> GetAll();
        public Task<Usuario> GetById(int id);
        public Task<Usuario> Update(int id, Usuario usuario);
        public Task<Usuario> SetRoleList(int id, ICollection<int> roleIdList);
    }

}
