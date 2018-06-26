using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace HwaTec.Blog.IRep
{
    public interface IRepository<T>
        where T : class
    {
        T GetById(object id);
        IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda);
        IQueryable<T> LoadPageEntities<S>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, S>> orderbyLambda, bool isAsc);
        bool Delete(T entity);
        void DeleteEntities(IEnumerable<T> entities);
        bool Update(T entity);
        T Add(T entity);
        int SaveChanges();
        IQueryable<T> Table { get; }
    }
}
