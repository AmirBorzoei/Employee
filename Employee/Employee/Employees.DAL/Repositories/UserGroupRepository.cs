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
        public UserGroup GetUserGroupByID(long id)
        {
            var context = GetDbContext();

            return Mapper.Map<UserGroup>(base.GetByID(context, id));
        }


        public List<UserGroup> GetUserGroups(SearchQuery<UserGroupEntity> searchQuery = null)
        {
            var context = GetDbContext();

            if (searchQuery == null)
                searchQuery = new SearchQuery<UserGroupEntity>();
            if (searchQuery.SortCriterias.Count == 0)
            {
                searchQuery.AddSortCriteria(new ExpressionSortCriteria<UserGroupEntity, string>(ug => ug.UserGroupName, SortDirection.Ascending));
                searchQuery.AddSortCriteria(new ExpressionSortCriteria<UserGroupEntity, long>(ug => ug.UserGroupId, SortDirection.Ascending));
            }

            var userGroupEntities = base.Get(context, searchQuery);
            return Mapper.Map<List<UserGroupEntity>, List<UserGroup>>(userGroupEntities);
        }

        public UserGroup UpdateOrInsert(UserGroup userGroup)
        {
            var context = GetDbContext();

            UserGroupEntity returnEntity = null;

            if (userGroup.State == ModelStates.New)
            {
                var userGroupEntity = Mapper.Map<UserGroupEntity>(userGroup);

                returnEntity = Insert(context, userGroupEntity);

                context.SaveChanges();
            }
            else if (userGroup.State == ModelStates.Modified)
            {
                var userGroupEntity = GetByID(context, userGroup.UserGroupId);

                Mapper.Map(userGroup, userGroupEntity);

                Update(context, userGroupEntity);

                context.SaveChanges();

                returnEntity = GetByID(context, userGroupEntity.UserGroupId);
            }

            return Mapper.Map<UserGroup>(returnEntity);
        }

        public void DeleteUserGroup(long id)
        {
            var context = GetDbContext();
            Delete(context, id);
            context.SaveChanges();
        }
    }
}