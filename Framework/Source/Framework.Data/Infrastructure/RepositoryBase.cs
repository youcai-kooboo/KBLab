using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using Framework.Data.Context;

namespace Framework.Data.Infrastructure
{
    public abstract class RepositoryBase<T> where T : class
    {
        private AppContext _appContext;
        private readonly IDbSet<T> _dbset;

        protected RepositoryBase(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
            _dbset = AppContext.Set<T>();
        }

        protected IDatabaseFactory DatabaseFactory
        {
            get;
            private set;
        }

        protected AppContext AppContext
        {
            get { return _appContext ?? (_appContext = DatabaseFactory.Get()); }
        }

        public virtual T Add(T entity)
        {
            _dbset.Add(entity);
           
            return entity;
        }
        public virtual void Update(T entity)
        {
            _dbset.Attach(entity);
            _appContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            _dbset.Remove(entity);
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = _dbset.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
            {
                _dbset.Remove(obj);
            }
        }

        public virtual T GetById(long id)
        {
            return _dbset.Find(id);
        }

        public virtual T GetById(string id)
        {
            return _dbset.Find(id);
        }

        public T Get(Expression<Func<T, bool>> where)
        {
            return _dbset.FirstOrDefault<T>(where);
        }

        public virtual IQueryable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return _dbset.Where(where);
        }

        public virtual IQueryable<T> GetAll()
        {
            return _dbset.AsQueryable();
        }

    }
}
