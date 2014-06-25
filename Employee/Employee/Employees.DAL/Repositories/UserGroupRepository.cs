using System.Data.Entity;
using System.Linq;
using AutoMapper;
using Caliburn.Micro;
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
        private readonly UserGroupPermissionRepository _userGroupPermissionRepository;


        public UserGroupRepository(PermissionKeyRepository permissionKeyRepository,
            UserGroupPermissionRepository userGroupPermissionRepository)
        {
            _permissionKeyRepository = permissionKeyRepository;
            _userGroupPermissionRepository = userGroupPermissionRepository;
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
                    MapPermissionKeys(context, userGroupEntity, userGroup.UserGroupPermissions);

                    returnEntity = Insert(context, userGroupEntity);

                    context.SaveChanges();
                }
                else if (userGroup.IsDirty)
                {
                    var userGroupEntity = GetUserGroupEntityFullByID(context, userGroup.UserGroupId);

                    Mapper.Map(userGroup, userGroupEntity);
                    MapPermissionKeys(context, userGroupEntity, userGroup.UserGroupPermissions);

                    Update(context, userGroupEntity);

                    context.SaveChanges();

                    returnEntity = GetUserGroupEntityFullByID(context, userGroupEntity.UserGroupId);
                }

                return Mapper.Map<UserGroup>(returnEntity);
            }
        }

        public void DeleteUserGroupById(long id)
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


        private void MapPermissionKeys(EmployeeContext context, UserGroupEntity userGroupEntity, BindableCollection<UserGroupPermission> userGroupPermissions)
        {
            if (userGroupEntity.UserGroupPermissions == null)
                userGroupEntity.UserGroupPermissions = new List<UserGroupPermissionEntity>();

            foreach (var userGroupPermission in userGroupPermissions)
            {
                switch (userGroupPermission.State)
                {
                    case ModelStates.New:
                        var addedUserGroupPermissionEntity = Mapper.Map<UserGroupPermissionEntity>(userGroupPermission);
                        addedUserGroupPermissionEntity.PermissionKeyEntity = _permissionKeyRepository.GetByID(context, userGroupPermission.PermissionKey.PermissionKeyId);
                        addedUserGroupPermissionEntity.UserGroupEntity = userGroupEntity;
                        userGroupEntity.UserGroupPermissions.Add(addedUserGroupPermissionEntity);
                        break;
                    case ModelStates.Modified:
                        var modifiedUserGroupPermissionEntity = userGroupEntity.UserGroupPermissions.First(upge => upge.UserGroupPermissionId == userGroupPermission.UserGroupPermissionId);
                        if (userGroupPermission.PermissionAccessType == PermissionAccessTypes.None)
                        {
                            _userGroupPermissionRepository.DeleteUserGroupPermissionById(modifiedUserGroupPermissionEntity.UserGroupPermissionId);
                        }
                        else
                        {
                            modifiedUserGroupPermissionEntity.PermissionKeyEntity = _permissionKeyRepository.GetByID(context, userGroupPermission.PermissionKey.PermissionKeyId);
                            modifiedUserGroupPermissionEntity.UserGroupEntity = userGroupEntity;
                            modifiedUserGroupPermissionEntity.PermissionAccessType = userGroupPermission.PermissionAccessType;
                        }
                        break;
                    case ModelStates.Deleted:
                        var deletedUserGroupPermissionEntity = userGroupEntity.UserGroupPermissions.First(upge => upge.UserGroupPermissionId == userGroupPermission.UserGroupPermissionId);
                        _userGroupPermissionRepository.DeleteUserGroupPermissionById(deletedUserGroupPermissionEntity.UserGroupPermissionId);
                        break;
                }
            }
        }
    }
}