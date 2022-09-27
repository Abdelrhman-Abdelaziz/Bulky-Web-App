using BulkyBook.DataAccess.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDBContext _db;
        private readonly DbSet<TEntity> dbSet;

        public Repository(ApplicationDBContext db)
        {
            _db = db;
            dbSet = db.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            dbSet.AddRange(entities);
        }
        // IncludeProp - "Category,CoverType"
        public TEntity Get(int? id)
        {
            return dbSet.Find(id);
        }

        public IEnumerable<TEntity> GetAll(string? includeProperties = null)
        {
            IQueryable<TEntity> query = AddIncludeProperties(dbSet, includeProperties);
            return query.ToList();
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate, string? includeProperties = null)
        {
            IQueryable<TEntity> query = dbSet.Where(predicate);
            query = AddIncludeProperties(query, includeProperties);
            return query.ToList();
        }

        public TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate, string? includeProperties = null, bool tracked = true)
        {
            IQueryable<TEntity> query = dbSet;
            if (!tracked)
                query = dbSet.AsNoTracking();

            query = query.Where(predicate);
            query = AddIncludeProperties(query, includeProperties);
            return query.FirstOrDefault();
        }

        public void Remove(TEntity entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            dbSet.RemoveRange(entities);
        }
        private IQueryable<TEntity> AddIncludeProperties(IQueryable<TEntity> query, string? includeProperties)
        {
            if (includeProperties != null)
            {
                var includeProps = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var includeProp in includeProps)
                {
                    query = query.Include(includeProp);
                }
            }
            return query;
        }
    }
}
