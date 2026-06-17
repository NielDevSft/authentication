using JWTAuthentication.Domain.Core.Interfaces;
using JWTAuthentication.Domain.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Linq.Expressions;

namespace JWTAuthentication.Persistence.Abstractions
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity<TEntity>
    {
        protected DbContext Db;
        protected DbSet<TEntity> DbSet;
        private readonly ILogger<Repository<TEntity>> _logger;

        public Repository(DbContext context, ILogger<Repository<TEntity>> logger)
        {
            Db = context;
            DbSet = Db.Set<TEntity>();
            _logger = logger;
        }

        public async Task AddAsync(TEntity obj, CancellationToken cancellationToken)
        {
            try
            {
                obj.CreateAt = DateTime.UtcNow;
                obj.UpdateAt = DateTime.UtcNow;
                await DbSet.AddAsync(obj, cancellationToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Dispose()
        {
            Db.Dispose();
        }

        public async Task<ICollection<TEntity>> FindAllAsync(CancellationToken cancellationToken, params string[] includes)
        {
            _logger.LogInformation($"Obtendo lista de {this.GetType().Name}");
            var query = DbSet.AsNoTracking();
            query = Includes(query, includes);

            var retorno = await query.ToListAsync(cancellationToken);
            _logger.LogInformation($"Lista de {this.GetType().Name} obtida");
            return retorno;
        }

        public async Task<ICollection<TEntity>> FindAllWhereAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken, params string[] includes)
        {
            _logger.LogInformation($"Obtendo lista de {this.GetType().Name}");
            var query = DbSet.AsNoTracking().Where(predicate);
            query = Includes(query, includes);
            var retorno = await query.ToListAsync(cancellationToken);
            _logger.LogInformation($"Lista de {this.GetType().Name} obtida");
            return retorno;
        }

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken, params string[] includes)
        {
            _logger.LogInformation($"Obtendo {this.GetType().Name}");

            var query = DbSet.AsNoTracking().Where(predicate);
            query = Includes(query, includes);
            var retorno = await query.FirstOrDefaultAsync(cancellationToken);

            _logger.LogInformation($"{this.GetType().Name} obtido");

            return retorno;
        }

        public async Task<TEntity?> GetByIdAsync(Guid uuid, CancellationToken cancellationToken, params string[] includes)
        {
            _logger.LogInformation($"Obtendo {this.GetType().Name}");

            var query = DbSet.AsNoTracking().Where(e => e.Uuid == uuid);
            query = Includes(query, includes);
            var retorno = await query.FirstOrDefaultAsync(cancellationToken);

            _logger.LogInformation($"{this.GetType().Name}, id {uuid} obtido");

            return retorno;
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return Db.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(TEntity obj, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Atualizando objeto {this.GetType().Name}, id {obj.Uuid}");
            obj.UpdateAt = DateTime.UtcNow;
            
            DbSet.Update(obj);
        }

        IQueryable<TEntity> Includes(IQueryable<TEntity> query, params string[] includes)
        {
            if (includes != null)
            {
                foreach (var item in includes)
                    query = query.Include(item);
            }

            return query;
        }

        protected object ConvertTableToObject(KeyValuePair<DataTable, string> retorno)
        {
            List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
            Dictionary<string, object> childRow;

            foreach (DataRow row in retorno.Key.Rows)
            {
                childRow = new Dictionary<string, object>();
                foreach (DataColumn col in retorno.Key.Columns)
                    childRow.Add(col.ColumnName, row[col]);

                parentRow.Add(childRow);
            }

            return parentRow;
        }

        public Task RemoveAsync(TEntity obj, CancellationToken cancellationToken)
        {
            if (obj is not null)
            {
                obj!.Removed = true;
                return UpdateAsync(obj, cancellationToken);
            }
            else
            {
                throw new Exception($"{this.GetType().Name} não encontrado para remoção");
            }
        }

        public async Task RemoveAsync(Guid uuid, CancellationToken cancellationToken)
        {
            var obj = await FirstOrDefaultAsync(ob => ob.Uuid == uuid, cancellationToken);
            if (obj is not null)
            {
                await RemoveAsync(obj, cancellationToken);
            }
            else
            {
                throw new Exception($"{this.GetType().Name} não encontrado para remoção");
            }
        }
    }
}
