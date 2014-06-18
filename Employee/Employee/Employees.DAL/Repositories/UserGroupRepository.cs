using System.Data.Entity;
using AutoMapper;
using Employees.DAL.Criteria;
using Employees.DAL.Entities;
using Employees.Shared.Models;
using System.Web.UI.WebControls;
using Employees.Shared.Constants;
using System.Collections.Generic;

namespace Employees.DAL.Repositories
{
    public class UserGroupRepository : GenericRepository<UserGroupEntity>
    {
        public UserGroupRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public List<UserGroup> GetUserGroups(SearchQuery<UserGroupEntity> searchQuery = null)
        {
            if (searchQuery == null)
                searchQuery = new SearchQuery<UserGroupEntity>();

            searchQuery.AddSortCriteria(new ExpressionSortCriteria<UserGroupEntity, string>(ug => ug.UserGroupName, SortDirection.Ascending));

            var userGroupEntities = base.Get(searchQuery);
            return Mapper.Map<List<UserGroupEntity>, List<UserGroup>>(userGroupEntities);
        }

        public UserGroup UpdateOrInsert(UserGroup userGroup)
        {
            var userGroupEntity = Mapper.Map<UserGroupEntity>(userGroup);

            if (userGroup.State == ModelStates.New)
                userGroupEntity = Insert(userGroupEntity);
            else if (userGroup.State == ModelStates.Modified)
                userGroupEntity = Update(userGroupEntity);

            return Mapper.Map<UserGroup>(userGroupEntity);
        }
    }
}