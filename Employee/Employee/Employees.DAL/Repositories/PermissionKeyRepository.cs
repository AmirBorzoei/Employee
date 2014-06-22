using AutoMapper;
using Employees.DAL.Criteria;
using Employees.DAL.Entities;
using Employees.Shared.Models;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace Employees.DAL.Repositories
{
    public class PermissionKeyRepository : GenericRepository<PermissionKeyEntity>
    {
        public List<PermissionKey> GetPermissionKeys(SearchQuery<PermissionKeyEntity> searchQuery = null)
        {
            using (var context = GetDbContext())
            {

                if (searchQuery == null)
                    searchQuery = new SearchQuery<PermissionKeyEntity>();
                if (searchQuery.SortCriterias.Count == 0)
                {
                    searchQuery.AddSortCriteria(new ExpressionSortCriteria<PermissionKeyEntity, long>(ug => ug.TreeId, SortDirection.Ascending));
                }

                var permissionKeyEntities = base.Get(context, searchQuery);
                return Mapper.Map<List<PermissionKeyEntity>, List<PermissionKey>>(permissionKeyEntities);
            }
        }
    }
}