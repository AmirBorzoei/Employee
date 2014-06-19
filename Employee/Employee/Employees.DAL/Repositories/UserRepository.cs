using System.Data.Entity;
using System.Linq;
using AutoMapper;
using Employees.DAL.Criteria;
using Employees.DAL.Entities;
using Employees.Shared.Constants;
using Employees.Shared.Models;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace Employees.DAL.Repositories
{
    public class UserRepository : GenericRepository<UserEntity>
    {
        private readonly UserGroupRepository _userGroupRepository;

        public UserRepository(DbContext dbContext, UserGroupRepository userGroupRepository) : base(dbContext)
        {
            _userGroupRepository = userGroupRepository;
        }

        public List<User> GetUsers(SearchQuery<UserEntity> searchQuery = null)
        {
            if (searchQuery == null)
                searchQuery = new SearchQuery<UserEntity>();

            searchQuery.AddSortCriteria(new ExpressionSortCriteria<UserEntity, string>(u => u.UserName, SortDirection.Ascending));
            searchQuery.AddSortCriteria(new ExpressionSortCriteria<UserEntity, long>(u => u.UserId, SortDirection.Ascending));

            var userEntities = base.Get(searchQuery);
            return Mapper.Map<List<UserEntity>, List<User>>(userEntities);
        }

        public User UpdateOrInsert(User user)
        {
            UserEntity returnEntity = null;

            if (user.State == ModelStates.New)
            {
                var userEntity = Mapper.Map<UserEntity>(user);
                ReplaceUserGroups(userEntity);

                returnEntity = Insert(userEntity);
            }
            else if (user.State == ModelStates.Modified)
            {
                var userEntity = GetByID(user.UserId);

                var userGroups = user.UserGroups;
                user.UserGroups = null;
                Mapper.Map(user, userEntity);
                
                FillUserGroups(userEntity, userGroups);

                returnEntity = Update(userEntity);
            }

            return Mapper.Map<User>(returnEntity);
        }


        private void ReplaceUserGroups(UserEntity userEntity)
        {
            var userGroups = new List<UserGroupEntity>();
            userEntity.UserGroups.ForEach(ug => userGroups.Add(_userGroupRepository.GetByID(ug.UserGroupId)));
            userEntity.UserGroups.Clear();
            userEntity.UserGroups.AddRange(userGroups);
        }

        private void FillUserGroups(UserEntity userEntity, List<UserGroup> userGroups)
        {
            var originalUserEntity = GetByID(userEntity.UserId);

            var addedUserGroupEntities = new List<UserGroupEntity>();
            foreach (var userGroup in userGroups)
            {
                if (originalUserEntity.UserGroups.Count(ug => ug.UserGroupId == userGroup.UserGroupId) == 0)
                {
                    addedUserGroupEntities.Add(_userGroupRepository.GetByID(userGroup.UserGroupId));
                }
            }
            foreach (var addedUserGroupEntity in addedUserGroupEntities)
            {
                userEntity.UserGroups.Add(addedUserGroupEntity);
            }
        }
    }
}