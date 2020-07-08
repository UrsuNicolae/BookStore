using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BookStore.BusinessLogic.Repository.IRepository;
using BookStore.Data.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookStore.BusinessLogic.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ILogger logger;
        private readonly ApplicationDbContext _db;
        internal DbSet<T> _dbSet;

        public Repository(ApplicationDbContext db, ILogger<UnitOfWork> logger)
        {
            _db = db;
            _dbSet = _db.Set<T>();
            this.logger = logger;
        }

        public bool Add(T entity)
        {
            try
            {
                _dbSet.Add(entity);
                return true;
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString());
                return false;
            }

        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null)
        {
            IQueryable<T> query = _dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                //egarloading
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            return query.ToList();
        }

        public T GetById(int Id)
        {
            return _dbSet.Find(Id);
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter = null,
            string includeProperties = null)
        {
            IQueryable<T> query = _dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                //egarloading
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return query.FirstOrDefault();
        }

        public bool Remove(int id)
        {
            try
            {
                T entity = _dbSet.Find(id);
                Remove(entity);
                return true;
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString());
                return false;
            }
        }

        public bool Remove(T entity)
        {
            try
            {
                _dbSet.Remove(entity);
                return true;
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString());
                return false;
            }

        }

        public bool RemoveRange(IEnumerable<T> entity)
        {
            try
            {
                _dbSet.RemoveRange(entity);
                return true;
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString());
                return false;
            }
        }
    }
}
