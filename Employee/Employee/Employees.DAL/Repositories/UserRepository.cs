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
        private readonly UserGroupRepository _userGroupRepository;


        public UserRepository(UserGroupRepository userGroupRepository)
        {
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

        public User ValidateUser(string userName, string password)
        {
            using (var context = GetDbContext())
            {
                var encryptPassword = Encryption.Encrypt(password);

                var searchQuery = new SearchQuery<UserEntity>();
                searchQuery.IncludeProperties = "UserGroups";
                searchQuery.Filters.Add(u => u.UserName == userName);
                searchQuery.Filters.Add(u => u.Password == encryptPassword);

                var userEntity = Get(context, searchQuery).FirstOrDefault();
                if (userEntity == null) return null;

                return Mapper.Map<UserEntity, User>(userEntity);
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
    }
}