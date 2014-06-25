using System.Linq;
using AutoMapper;
using Employees.DAL.Criteria;
using Employees.DAL.Entities;
using Employees.Shared.Constants;
using Employees.Shared.Models;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Employees.Shared.Permission;

namespace Employees.DAL.Repositories
{
    public class UserRepository : GenericRepository<UserEntity>
    {
        private const string SuperAdminUserName =
            "AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAt/Si262AgkWY9D6L6U8UkQAAAAACAAAAAAAQZgAAAAEAACAAAAB8eB5DVCtKW+rEROUWjuAZiK2ZuWyZxsHUQRelh4cxfQAAAAAOgAAAAAIAACAAAADjduNMQ7iBQx9qIVz5oI6E276pgJ+9TqediLruJpOMIhAAAAAexY0KMg93FlH7HE54IyP8QAAAAHv5VqNwntcJs0RnW+3QDvodoOWw8Kut1hY6BjoPaiOgyTAdc+1mzgpeJYEhPzJAjiOPO441bP3KZy6WOCKE1oQ=";

        private const string SuperAdminPassword =
            "AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAt/Si262AgkWY9D6L6U8UkQAAAAACAAAAAAAQZgAAAAEAACAAAADgVfYTD17yhxgJmQYJXilcQpgvIy5zeYM9gO94WTRS0gAAAAAOgAAAAAIAACAAAACz0NAtcYleR79675KLF9jrSTfgXX2k3VvnwUBiG+xxThAAAAAUuLZo6skuAt1dmeW7nqJqQAAAAJBC+dqJSen7UQydlo4miK76R5v1mkPqyDwmX9opCVagTYc4ekmDQmhHZmwdl/4vIXkfmunsTcojTnzcTuy4nKo=";


        private readonly PermissionKeyRepository _permissionKeyRepository;
        private readonly UserGroupRepository _userGroupRepository;


        public UserRepository(PermissionKeyRepository permissionKeyRepository,
            UserGroupRepository userGroupRepository)
        {
            _permissionKeyRepository = permissionKeyRepository;
            _userGroupRepository = userGroupRepository;
        }


        public List<User> GetUsers(SearchQuery<UserEntity> searchQuery = null)
        {
            using (var context = GetDbContext())
            {
                if (searchQuery == null)
                    searchQuery = new SearchQuery<UserEntity>();

                searchQuery.AddSortCriteria(new ExpressionSortCriteria<UserEntity, string>(u => u.UserName, SortDirection.Ascending));
                searchQuery.AddSortCriteria(new ExpressionSortCriteria<UserEntity, long>(u => u.UserId, SortDirection.Ascending));

                var userEntities = base.Get(context, searchQuery);
                return Mapper.Map<List<UserEntity>, List<User>>(userEntities);
            }
        }

        public User UpdateOrInsert(User user)
        {
            var context = GetDbContext();

            UserEntity returnEntity = null;

            if (user.State == ModelStates.New)
            {
                var userEntity = Mapper.Map<UserEntity>(user);
                ReplaceUserGroups(context, userEntity);

                returnEntity = Insert(context, userEntity);

                context.SaveChanges();
            }
            else if (user.State == ModelStates.Modified)
            {
                var userEntity = GetByID(context, user.UserId);

                var userGroups = user.UserGroups;
                user.UserGroups = null;
                Mapper.Map(user, userEntity);

                FillUserGroups(context, userEntity, userGroups);

                returnEntity = Update(context, userEntity);

                context.SaveChanges();
            }

            return Mapper.Map<User>(returnEntity);
        }

        public LoginedUser ValidateUser(string userName, string password)
        {
            using (var context = GetDbContext())
            {
                if (userName == Encryption.Decrypt(SuperAdminUserName) && password == Encryption.Decrypt(SuperAdminPassword))
                {
                    return SuperAdminUser(context);
                }

                var searchQuery = new SearchQuery<UserEntity>();
                searchQuery.IncludeProperties = "UserGroups,UserGroups.UserGroupPermissions,UserGroups.UserGroupPermissions.PermissionKeyEntity";
                searchQuery.Filters.Add(u => u.UserName == userName);

                var userEntity = Get(context, searchQuery).FirstOrDefault();
                if (userEntity != null)
                {
                    var user = Mapper.Map<UserEntity, User>(userEntity);
                    if (user.Password == password)
                    {
                        var userPermissions = GetUserPermissions(context, userEntity);
                        var loginedUser = new LoginedUser {User = user};
                        loginedUser.Permissions.AddRange(userPermissions);

                        return loginedUser;
                    }
                }

                return null;
            }
        }


        private UserEntity GetUserEntityWithUserGroupsByID(EmployeeContext context, long id)
        {
            var searchQuery = new SearchQuery<UserEntity>();
            searchQuery.IncludeProperties = "UserGroups";
            searchQuery.Filters.Add(u => u.UserId == id);

            return Get(context, searchQuery).First();
        }

        private void ReplaceUserGroups(EmployeeContext context, UserEntity userEntity)
        {
            var userGroups = new List<UserGroupEntity>();
            userEntity.UserGroups.ForEach(ug => userGroups.Add(_userGroupRepository.GetByID(context, ug.UserGroupId)));
            userEntity.UserGroups.Clear();
            userEntity.UserGroups.AddRange(userGroups);
        }

        private void FillUserGroups(EmployeeContext context, UserEntity userEntity, List<UserGroup> userGroups)
        {
            var originalUserEntity = GetUserEntityWithUserGroupsByID(context, userEntity.UserId);

            var addedUserGroupEntities = new List<UserGroupEntity>();
            foreach (var userGroup in userGroups)
            {
                if (originalUserEntity.UserGroups.Count(ug => ug.UserGroupId == userGroup.UserGroupId) == 0)
                {
                    addedUserGroupEntities.Add(_userGroupRepository.GetByID(context, userGroup.UserGroupId));
                }
            }
            foreach (var addedUserGroupEntity in addedUserGroupEntities)
            {
                userEntity.UserGroups.Add(addedUserGroupEntity);
            }

            var deletedUserGroupEntities = new List<UserGroupEntity>();
            foreach (var originalUserGroupEntity in originalUserEntity.UserGroups)
            {
                if (userGroups.Count(ug => ug.UserGroupId == originalUserGroupEntity.UserGroupId) == 0)
                {
                    deletedUserGroupEntities.Add(originalUserGroupEntity);
                }
            }
            foreach (var deletedUserGroupEntity in deletedUserGroupEntities)
            {
                userEntity.UserGroups.Remove(deletedUserGroupEntity);
            }
        }

        private List<UserPermission> GetUserPermissions(EmployeeContext context, UserEntity userEntity)
        {
            var userPermissions = new List<UserPermission>();

            if (userEntity.UserGroups == null)
                return userPermissions;

            var allPermissionKeys = _permissionKeyRepository.Get(context);

            foreach (var userGroupEntity in userEntity.UserGroups)
            {
                foreach (var userGroupPermissionEntity in userGroupEntity.UserGroupPermissions)
                {
                    var permissionKeyFullName = GetPermissionKeyFullName(userGroupPermissionEntity.PermissionKeyEntity, allPermissionKeys);

                    var userPermission = userPermissions.FirstOrDefault(up => up.PermissionKeyFullName == permissionKeyFullName);
                    if (userPermission == null)
                    {
                        userPermission = new UserPermission {PermissionKeyFullName = permissionKeyFullName};
                        userPermissions.Add(userPermission);
                    }

                    if ((int) userPermission.PermissionAccessType < (int) userGroupPermissionEntity.PermissionAccessType)
                    {
                        userPermission.PermissionAccessType = userGroupPermissionEntity.PermissionAccessType;
                    }
                }
            }

            return userPermissions;
        }

        private string GetPermissionKeyFullName(PermissionKeyEntity permissionKey, List<PermissionKeyEntity> allPermissionKeys)
        {
            var parentPermissionKey = allPermissionKeys.FirstOrDefault(pk => pk.TreeId == permissionKey.TreeParentId);

            if (parentPermissionKey == null)
                return permissionKey.PermissionKeyName;

            var permissionKeyFullName = GetPermissionKeyFullName(parentPermissionKey, allPermissionKeys) + "_" + permissionKey.PermissionKeyName;

            return permissionKeyFullName;
        }

        private LoginedUser SuperAdminUser(EmployeeContext context)
        {
            var superAdminUser = new LoginedUser();
            superAdminUser.User = new User
            {
                UserId = long.MinValue,
                FirstName = "Super",
                LastName = "Admin",
                UserName = "SuperAdminUser",
            };

            var allPermissionKeys = _permissionKeyRepository.Get(context);
            foreach (var permissionKeyEntity in allPermissionKeys)
            {
                superAdminUser.Permissions.Add(new UserPermission
                {
                    PermissionKeyFullName = GetPermissionKeyFullName(permissionKeyEntity, allPermissionKeys),
                    PermissionAccessType = PermissionAccessTypes.Active,
                });
            }

            return superAdminUser;
        }
    }
}