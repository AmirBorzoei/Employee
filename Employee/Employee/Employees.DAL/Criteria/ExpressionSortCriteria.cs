using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.UI.WebControls;

namespace Employees.DAL.Criteria
{
    public class ExpressionSortCriteria<T, TKey> : ISortCriteria<T>
    {
        public ExpressionSortCriteria()
        {
            Direction = SortDirection.Ascending;
        }

        public ExpressionSortCriteria(Expression<Func<T, TKey>> expression, SortDirection direction)
        {
            SortExpression = expression;
            Direction = direction;
        }

        public Expression<Func<T, TKey>> SortExpression { get; set; }

        public SortDirection Direction { get; set; }

        public IOrderedQueryable<T> ApplyOrdering(IQueryable<T> query, Boolean useThenBy)
        {
            IOrderedQueryable<T> result = null;
            if (SortExpression != null)
            {
                if (Direction == SortDirection.Ascending)
                {
                    result = !useThenBy ? query.OrderBy(SortExpression) : ((IOrderedQueryable<T>) query).ThenBy(SortExpression);
                }
                else
                {
                    result = !useThenBy ? query.OrderByDescending(SortExpression) : ((IOrderedQueryable<T>) query).ThenBy(SortExpression);
                }
            }
            else
            {
                return query.OrderBy(x => x);
            }
            return result;
        }
    }
}