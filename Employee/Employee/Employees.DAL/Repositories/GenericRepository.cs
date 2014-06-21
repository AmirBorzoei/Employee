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
        protected virtual List<TEntity> Get(EmployeeContext context, SearchQuery<TEntity> searchQuery = null)
        {
            var dbSet = GetDbSet(context);

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

        internal virtual TEntity GetByID(EmployeeContext context, object id)
        {
            var dbSet = GetDbSet(context);
            return dbSet.Find(id);
        }

        protected virtual TEntity Insert(EmployeeContext context, TEntity entity)
        {
            var dbSet = GetDbSet(context);
            var insertedTEntity = dbSet.Add(entity);
            return insertedTEntity;
        }

        protected virtual void Delete(EmployeeContext context, object id)
        {
            var dbSet = GetDbSet(context);
            var entityToDelete = dbSet.Find(id);
            Delete(context, entityToDelete);
        }

        protected virtual void Delete(EmployeeContext context, TEntity entityToDelete)
        {
            var dbSet = GetDbSet(context);

            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        protected virtual TEntity Update(EmployeeContext context, TEntity entityToUpdate)
        {
            if (context == null)
                context = GetDbContext();
            var dbSet = GetDbSet(context);

            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
            
            return entityToUpdate;
        }


        protected EmployeeContext GetDbContext()
        {
            var context = IoC.Get<EmployeeContext>();
            return context;
        }
        
        protected DbSet<TEntity> GetDbSet(EmployeeContext context)
        {
            var dbSet = context.Set<TEntity>();
            return dbSet;
        }
    }
}