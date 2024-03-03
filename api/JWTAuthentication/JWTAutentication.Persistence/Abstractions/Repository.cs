using JWTAuthentication.Domain.Core.Interfaces;
using JWTAuthentication.Domain.Core.Models;
using JWTAuthentication.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace JWTAuthentication.Persistence.Abstractions
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity<TEntity>
    {
        protected AuthenticationOrganizationContext Db;
        protected DbSet<TEntity> DbSet;

        public Repository(AuthenticationOrganizationContext context)
        {
            Db = context;
            DbSet = Db.Set<TEntity>();
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

        public IEnumerable<TEntity> FindAll(params string[] includes)
        {
            var query = DbSet.AsNoTracking();
            query = Includes(query, includes);

            var retorno = query.ToList();

            return retorno;
        }

        public IEnumerable<TEntity> FindAllWhere(Expression<Func<TEntity, bool>> predicate, params string[] includes)
        {
            var query = DbSet.AsNoTracking().Where(predicate);
            query = Includes(query, includes);

            return query.ToList();
        }

        public TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> predicate, params string[] includes)
        {
            var query = DbSet.AsNoTracking().Where(predicate);
            query = Includes(query, includes);

            return query.FirstOrDefault();
        }

        public TEntity? GetById(int id, params string[] includes)
        {
            var query = DbSet.AsNoTracking().Where(e => e.Id == id);
            query = Includes(query, includes);

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

        public async Task<IEnumerable<TEntity>> FindAllWhereAsync(Expression<Func<TEntity, bool>> predicate, params string[] includes)
        {
            var query = DbSet.AsNoTracking().Where(predicate);
            query = Includes(query, includes);

            return query.ToList();
        }
    }
}
