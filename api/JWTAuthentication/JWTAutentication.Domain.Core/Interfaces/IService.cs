namespace JWTAuthentication.Domain.Core.Interfaces
{
    public interface IService<T>
    {
        Task<List<T>> GetAll();
        Task<T> GetById(int id);
        Task<T> Create(T item);
        Task<T> Update(int id, T item);
        Task Delete(int id);
    }
}
