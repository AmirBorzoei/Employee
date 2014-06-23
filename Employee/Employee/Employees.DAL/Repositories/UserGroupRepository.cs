using System.Data.Entity;
using System.Linq;
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
        private readonly PermissionKeyRepository _permissionKeyRepository;


        public UserGroupRepository(PermissionKeyRepository permissionKeyRepository)
        {
            _permissionKeyRepository = permissionKeyRepository;
        }


        public UserGroup GetUserGroupFullByID(long id)
        {
            using (var context = GetDbContext())
            {
                return Mapper.Map<UserGroup>(GetUserGroupEntityFullByID(context, id));
            }
        }

        public List<UserGroup> GetUserGroups(SearchQuery<UserGroupEntity> searchQuery = null)
        {
            using (var context = GetDbContext())
            {
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
        }

        public UserGroup UpdateOrInsert(UserGroup userGroup)
        {
            using (var context = GetDbContext())
            {
                UserGroupEntity returnEntity = null;

                if (userGroup.State == ModelStates.New)
                {
                    var userGroupEntity = Mapper.Map<UserGroupEntity>(userGroup);

                    ReplacePermissionKeys(context, userGroupEntity);

                    returnEntity = Insert(context, userGroupEntity);

                    context.SaveChanges();
                }
                else if (userGroup.State == ModelStates.Modified)
                {
                    var userGroupEntity = GetUserGroupEntityFullByID(context, userGroup.UserGroupId);

                    var userGroupPermissions = userGroup.UserGroupPermissions.ToList();
                    userGroup.UserGroupPermissions.Clear();
                    Mapper.Map(userGroup, userGroupEntity);

                    FillPermissionKeys(context, userGroupEntity, userGroupPermissions);

                    Update(context, userGroupEntity);

                    context.SaveChanges();

                    returnEntity = GetUserGroupEntityFullByID(context, userGroupEntity.UserGroupId);
                }

                return Mapper.Map<UserGroup>(returnEntity);
            }
        }

        public void DeleteUserGroup(long id)
        {
            using (var context = GetDbContext())
            {
                Delete(context, id);
                context.SaveChanges();
            }
        }


        public UserGroupEntity GetUserGroupEntityFullByID(EmployeeContext context, long id)
        {
            var searchQuery = new SearchQuery<UserGroupEntity>();
            searchQuery.IncludeProperties = "UserGroupPermissions,UserGroupPermissions.PermissionKeyEntity,UserGroupPermissions.UserGroupEntity";
            searchQuery.Filters.Add(u => u.UserGroupId == id);

            return base.Get(context, searchQuery).FirstOrDefault();
        }

        private void ReplacePermissionKeys(EmployeeContext context, UserGroupEntity userGroupEntity)
        {
            userGroupEntity.UserGroupPermissions.ForEach(ugp => ugp.PermissionKeyEntity = _permissionKeyRepository.GetByID(context, ugp.PermissionKeyEntity.PermissionKeyId));
        }

        private void FillPermissionKeys(EmployeeContext context, UserGroupEntity userGroupEntity, List<UserGroupPermission> userGroupPermissions)
        {
            var originalUserGroupEntity = GetUserGroupEntityFullByID(context, userGroupEntity.UserGroupId);

            var addedUserGroupPermissions = new List<UserGroupPermissionEntity>();
            foreach (var userGroupPermission in userGroupPermissions)
            {
                if (userGroupPermission.PermissionAccessType == PermissionAccessTypes.None) continue;

                if (originalUserGroupEntity.UserGroupPermissions.Count(ugpe => ugpe.PermissionKeyEntity.PermissionKeyId == userGroupPermission.PermissionKey.PermissionKeyId) == 0)
                {
                    var userGroupPermissionEntity = Mapper.Map<UserGroupPermissionEntity>(userGroupPermission);
                    userGroupPermissionEntity.PermissionKeyEntity = _permissionKeyRepository.GetByID(context, userGroupPermission.PermissionKey.PermissionKeyId);
                    userGroupPermissionEntity.UserGroupEntity = userGroupEntity;
                    addedUserGroupPermissions.Add(userGroupPermissionEntity);
                }
            }
            userGroupEntity.UserGroupPermissions.AddRange(addedUserGroupPermissions);

            var deletedUserGroupPermissions = new List<UserGroupPermissionEntity>();
            foreach (var originalUserGroupPermissionEntity in originalUserGroupEntity.UserGroupPermissions)
            {
                var userGroupPermission = userGroupPermissions.FirstOrDefault(ugp => ugp.PermissionKey.PermissionKeyId == originalUserGroupPermissionEntity.PermissionKeyEntity.PermissionKeyId);
                if (userGroupPermission == null || userGroupPermission.PermissionAccessType == PermissionAccessTypes.None)
                {
                    deletedUserGroupPermissions.Add(originalUserGroupPermissionEntity);
                }
            }
            deletedUserGroupPermissions.ForEach(ugpe => userGroupEntity.UserGroupPermissions.Remove(ugpe));
        }
    }
}