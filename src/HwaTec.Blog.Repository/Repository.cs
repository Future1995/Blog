using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace HwaTec.Blog.Repository
{
    public class Repository<T> : IRepository<T>
        where T : class, new()
    {
        private readonly DbContext _context;
        private DbSet<T> _entities;

        public Repository(DbContext context)
        {
            _context = context;
        }

        public T AddEntity(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                Entities.Add(entity);
                _context.SaveChanges();
            }
            catch (Exception dbEx)
            {
                //ensure that the detailed error text is saved in the Log
                //throw new Exception(GetFullErrorTextAndRollbackEntityChanges(dbEx), dbEx);
            }
            return entity;
        }

        public bool DeleteEntity(T entity)
        {
            _context.Entry<T>(entity).State = EntityState.Modified;
            return _context.SaveChanges() > 0;
        }

        public IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda)
        {
            return _context.Set<T>().Where(whereLambda);
        }

        public IQueryable<T> LoadPageEntities<S>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, S>> orderbyLambda, bool isAsc)
        {

            var temp = whereLambda == null ? _context.Set<T>().Where(x => true) : _context.Set<T>().Where(whereLambda);
            totalCount = temp.Count();
            if (isAsc)
            {
                temp = temp.OrderBy(orderbyLambda).Skip(pageSize * (pageIndex - 1)).Take(pageSize).OrderBy(orderbyLambda);
            }
            else
            {
                temp = temp.OrderByDescending(orderbyLambda).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
            }
            return temp;
        }

        public bool UpdateEntity(T entity)
        {
            _context.Entry<T>(entity).State = EntityState.Modified;
            return _context.SaveChanges() > 0;
        }

        public T GetById(object id)
        {
            return Entities.Find(id);
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        protected virtual DbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = _context.Set<T>();
                return _entities;
            }
        }

        public virtual IQueryable<T> Table
        {
            get
            {
                return Entities;
            }
        }

    }
}
