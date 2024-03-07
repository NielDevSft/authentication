using JWTAuthentication.Domain.Core.Interfaces;
using JWTAuthentication.Domain.Core.Models;
using JWTAuthentication.Persistence.Contexts;
using JWTAuthentication.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Linq.Expressions;

namespace JWTAuthentication.Persistence.Abstractions
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity<TEntity>
    {
        protected AuthenticationOrganizationContext Db;
        protected DbSet<TEntity> DbSet;
        private AuthenticationOrganizationContext context;
        private readonly ILogger<RoleRepository> _logger;

        public Repository(AuthenticationOrganizationContext context, ILogger<RoleRepository> logger)
        {
            Db = context;
            DbSet = Db.Set<TEntity>();
            _logger = logger;
        }

        protected Repository(AuthenticationOrganizationContext context)
        {
            this.context = context;
        }

        public void Add(TEntity obj)
        {
            try
            {
                obj.CreateAt = DateTime.UtcNow;
                obj.UpdateAt = DateTime.UtcNow;
                DbSet.Add(obj);
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

        public ICollection<TEntity> FindAll(params string[] includes)
        {
            _logger.LogInformation($"Obtendo lista de {this.GetType().Name}");
            var query = DbSet.AsNoTracking();
            query = Includes(query, includes);

            var retorno = query.ToList();
            _logger.LogInformation($"Lista de {this.GetType().Name} obtida");
            return retorno;
        }

        public ICollection<TEntity> FindAllWhere(Expression<Func<TEntity, bool>> predicate, params string[] includes)
        {
            _logger.LogInformation($"Obtendo lista de {this.GetType().Name}");
            var query = DbSet.AsNoTracking().Where(predicate);
            query = Includes(query, includes);
            _logger.LogInformation($"Lista de {this.GetType().Name} obtida");
            return query.ToList();
        }

        public TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> predicate, params string[] includes)
        {
            _logger.LogInformation($"Obtendo {this.GetType().Name}");
            var query = DbSet.AsNoTracking().Where(predicate);
            query = Includes(query, includes);
            _logger.LogInformation($"{this.GetType().Name} obtido");
            return query.FirstOrDefault();
        }

        public TEntity? GetById(int id, params string[] includes)
        {
            _logger.LogInformation($"Obtendo {this.GetType().Name}");
            var query = DbSet.AsNoTracking().Where(e => e.Id == id);
            query = Includes(query, includes);
            _logger.LogInformation($"{this.GetType().Name}, id {id} obtido");
            return query.FirstOrDefault();
        }

        public void Remove(int id)
        {
            var obj = GetById(id);
            if (obj! != null)
            {
                obj!.Removed = true;
                Update(obj);
            }
        }

        public int SaveChanges()
        {
            return Db.SaveChanges();

        }

        public void Update(TEntity obj)
        {
            _logger.LogInformation($"Atualizando objeto {this.GetType().Name}, id {obj.Id}");
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

        public async Task<ICollection<TEntity>> FindAllWhereAsync(Expression<Func<TEntity, bool>> predicate, params string[] includes)
        {
            _logger.LogInformation($"Obtendo lista de {this.GetType().Name}");
            var query = DbSet.AsNoTracking().Where(predicate);
            query = Includes(query, includes);
            _logger.LogInformation($"Lista de {this.GetType().Name} obtida");
            return query.ToList();
        }

        public async Task<TEntity?> GetByIdAsync(int id, params string[] includes)
        {
            _logger.LogInformation($"Obtendo {this.GetType().Name}, id {id}");
            var query = DbSet.AsNoTracking().Where(e => e.Id == id);
            query = Includes(query, includes);
            _logger.LogInformation($"{this.GetType().BaseType}, id {id} obtido");
            return query.FirstOrDefault();
        }
    }
}
