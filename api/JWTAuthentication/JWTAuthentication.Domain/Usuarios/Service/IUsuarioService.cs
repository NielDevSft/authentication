namespace JWTAuthentication.Domain.Usuarios.Service
{
    public interface IUsuarioService
    {
        public Task<Usuario> Create(Usuario usuario);
        public Task Delete(Guid Uuid);
        public Task<List<Usuario>> GetAll();
        public Task<Usuario> GetById(Guid Uuid);
        public Task<Usuario> Update(Guid Uuid, Usuario usuario);
        public Task<Usuario> SetRoleList(Guid Uuid, ICollection<Guid> roleIdList);
    }

}
