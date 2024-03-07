using JWTAuthentication.Domain.Core.Models;
using System.Linq.Expressions;

namespace JWTAuthentication.Domain.Core.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity<TEntity>
    {
        void Add(TEntity obj);

        void Update(TEntity obj);
        TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> predicate, params string[] includes);
        ICollection<TEntity> FindAll(params string[] includes);
        ICollection<TEntity> FindAllWhere(Expression<Func<TEntity, bool>> predicate, params string[] includes);
        void Remove(int id);
        int SaveChanges();
        TEntity? GetById(int id, params string[] includes);
        Task<TEntity?> GetByIdAsync(int id, params string[] includes);
        Task<ICollection<TEntity>> FindAllWhereAsync(Expression<Func<TEntity, bool>> predicate, params string[] includes);
    }
}

