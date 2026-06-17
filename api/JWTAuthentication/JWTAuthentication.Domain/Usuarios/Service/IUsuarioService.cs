namespace JWTAuthentication.Domain.Usuarios.Service
{
    public interface IUsuarioService
    {
        public Task<Usuario> Create(Usuario usuario, CancellationToken cancellationToken);
        public Task Delete(Guid Uuid, CancellationToken cancellationToken);
        public Task<List<Usuario>> GetAll(CancellationToken cancellationToken);
        public Task<Usuario> GetById(Guid Uuid, CancellationToken cancellationToken);
        public Task<Usuario> Update(Guid Uuid, Usuario usuario, CancellationToken cancellationToken);
        public Task<Usuario> SetRoleList(Guid Uuid, ICollection<Guid> roleIdList, CancellationToken cancellationToken);
    }

}
