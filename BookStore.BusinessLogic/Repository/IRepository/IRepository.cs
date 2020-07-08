using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BookStore.BusinessLogic.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        bool Add(T entity);
        bool Remove(int id);
        bool Remove(T entity);
        bool RemoveRange(IEnumerable<T> entity);
        IEnumerable<T> GetAll(
            Expression<Func<T,bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>orderBy = null,
            string includeProperties = null
            );
        T GetFirstOrDefault(
            Expression<Func<T, bool>> filter = null,
            string includeProperties = null
            );
        T GetById(int Id);
    }
}
