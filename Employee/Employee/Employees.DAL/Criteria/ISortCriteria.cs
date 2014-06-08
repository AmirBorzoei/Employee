using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace Employees.DAL.Criteria
{
    public interface ISortCriteria<T>
    {
        SortDirection Direction { get; set; }

        IOrderedQueryable<T> ApplyOrdering(IQueryable<T> query, Boolean useThenBy);
    }
}