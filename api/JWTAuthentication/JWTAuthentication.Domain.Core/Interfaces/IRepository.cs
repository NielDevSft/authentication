using JWTAuthentication.Domain.Core.Models;
using System.Linq.Expressions;

namespace JWTAuthentication.Domain.Core.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity<TEntity>
    {
        Task AddAsync(TEntity obj, CancellationToken cancellationToken);
        Task UpdateAsync(TEntity obj, CancellationToken cancellationToken);
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken, params string[] includes);
        Task<ICollection<TEntity>> FindAllAsync(CancellationToken cancellationToken, params string[] includes);
        Task RemoveAsync(Guid uuid, CancellationToken cancellationToken);
        Task RemoveAsync(TEntity obj, CancellationToken cancellationToken);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<TEntity?> GetByIdAsync(Guid uuid, CancellationToken cancellationToken, params string[] includes);
        Task<ICollection<TEntity>> FindAllWhereAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken, params string[] includes);
    }
}

