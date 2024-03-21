namespace JWTAuthentication.Domain.Core.Interfaces
{
    public interface IService<T>
    {
        Task<List<T>> GetAll();
        Task<T> GetById(Guid uuid);
        Task<T> Create(T item);
        Task<T> Update(Guid uuid, T item);
        Task Delete(Guid uuid);
    }
}
