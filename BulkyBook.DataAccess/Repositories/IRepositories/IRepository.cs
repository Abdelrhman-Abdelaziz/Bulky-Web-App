using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repositories.IRepositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Get(int? id);
        IEnumerable<TEntity> GetAll(string? includeProperties = null);
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate, string? includeProperties = null);
        TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate, string? includeProperties = null, bool tracked = true);

        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);

    }
}
