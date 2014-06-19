using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Caliburn.Micro;
using Employees.DAL.Criteria;

namespace Employees.DAL.Repositories
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext _dbContext;


        protected GenericRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }


        protected virtual List<TEntity> Get(SearchQuery<TEntity> searchQuery = null)
        {
            var dbSet = GetDbSet();
            IQueryable<TEntity> query = dbSet;

            if (searchQuery != null && searchQuery.Filters != null && searchQuery.Filters.Count > 0)
            {
                foreach (var filter in searchQuery.Filters)
                {
                    query = query.Where(filter);
                }
            }

            if (searchQuery != null && searchQuery.IncludeProperties != null && searchQuery.IncludeProperties.Length > 0)
            {
                foreach (string includeProperty in searchQuery.IncludeProperties.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (searchQuery != null && searchQuery.SortCriterias != null && searchQuery.SortCriterias.Count > 0)
            {
                var sortCriteria = searchQuery.SortCriterias[0];
                var orderedSequence = sortCriteria.ApplyOrdering(query, false);

                if (searchQuery.SortCriterias.Count > 1)
                {
                    for (var i = 1; i < searchQuery.SortCriterias.Count; i++)
                    {
                        var sc = searchQuery.SortCriterias[i];
                        orderedSequence = sc.ApplyOrdering(orderedSequence, true);
                    }
                }
                return orderedSequence.ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        //public virtual List<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
        //                                 Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        //                                 string includeProperties = "")
        //{
        //    var dbSet = GetDbSet();

        //    IQueryable<TEntity> query = dbSet;

        //    if (filter != null)
        //    {
        //        query = query.Where(filter);
        //    }

        //    foreach (string includeProperty in includeProperties.Split
        //        (new[] {','}, StringSplitOptions.RemoveEmptyEntries))
        //    {
        //        query = query.Include(includeProperty);
        //    }

        //    if (orderBy != null)
        //    {
        //        return orderBy(query).ToList();
        //    }
        //    else
        //    {
        //        return query.ToList();
        //    }
        //}

        public virtual TEntity GetByID(object id)
        {
            var dbSet = GetDbSet();
            return dbSet.Find(id);
        }

        protected virtual TEntity Insert(TEntity entity)
        {
            var context = GetDbContext();
            var dbSet = GetDbSet(context);
            var insertedTEntity = dbSet.Add(entity);

            context.SaveChanges();

            return insertedTEntity;
        }

        public virtual void Delete(object id)
        {
            var dbSet = GetDbSet();
            var entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        protected virtual void Delete(TEntity entityToDelete)
        {
            var context = GetDbContext();
            var dbSet = GetDbSet(context);
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);

            context.SaveChanges();
        }

        protected virtual TEntity Update(TEntity entityToUpdate)
        {
            var context = GetDbContext();
            var dbSet = GetDbSet(context);
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;

            context.SaveChanges();

            context.Entry(entityToUpdate).Reload();

            return entityToUpdate;
        }


        private DbContext GetDbContext()
        {
            //var context = IoC.Get<EmployeeContext>();
            //return context;

            return _dbContext;
        }

        protected DbSet<TEntity> GetDbSet()
        {
            var context = GetDbContext();
            return GetDbSet(context);
        }

        private DbSet<TEntity> GetDbSet(DbContext context)
        {
            var dbSet = context.Set<TEntity>();
            return dbSet;
        }
    }
}