using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Employees.DAL.Repositories;
using Employees.Shared.Constants;
using Employees.Shared.Interfaces;
using Employees.Shared.Models;
using Employees.Shared.ViewModels;

namespace Employees.Administration.ViewModels
{
    public interface IUserGroupViewModel : IScreenBase, ISupportNew, ISupportSave, ISupportReload
    {
        UserGroup CurrentObject { get; set; }
    }

    public class UserGroupViewModel : ScreenBase<UserGroup>, IUserGroupViewModel
    {
        private readonly IEmployeeUnitOfWork _employeeUnitOfWork;


        public UserGroupViewModel(IEmployeeUnitOfWork employeeUnitOfWork)
        {
            _employeeUnitOfWork = employeeUnitOfWork;

            UserGroupPermissions = new BindableCollection<UserGroupPermission>();
        }


        public BindableCollection<UserGroupPermission> UserGroupPermissions { get; private set; }


        protected override void CurrentObjectChanged()
        {
            base.CurrentObjectChanged();

            FillUserGroupPermissions();
        }


        public void New()
        {
            CurrentObject = new UserGroup();
        }

        public void Save()
        {
            if (CurrentObject == null) return;

            RefreshUserGroupPermissionsOnCurrentObject();
            CurrentObject = _employeeUnitOfWork.UserGroupRepository.UpdateOrInsert(CurrentObject);
        }

        public void Reload()
        {
            if (CurrentObject == null || CurrentObject.State == ModelStates.New) return;

            CurrentObject = _employeeUnitOfWork.UserGroupRepository.GetUserGroupByID(CurrentObject.UserGroupId);
        }


        private void FillUserGroupPermissions()
        {
            UserGroupPermissions.Clear();

            var allUserGroupPermissions = new List<UserGroupPermission>();
            var allPermissionKeys = _employeeUnitOfWork.PermissionKeyRepository.GetPermissionKeys();

            foreach (var permissionKey in allPermissionKeys)
            {
                var currentUserGroupPermission = CurrentObject == null
                    ? null
                    : CurrentObject.UserGroupPermissions.FirstOrDefault(ugp => ugp.PermissionKey.PermissionKeyId == permissionKey.PermissionKeyId);

                allUserGroupPermissions.Add(new UserGroupPermission
                {
                    UserGroupPermissionId = currentUserGroupPermission == null ? 0 : currentUserGroupPermission.UserGroupPermissionId,
                    UserGroup = CurrentObject,
                    PermissionKey = permissionKey,
                    TreeId = permissionKey.TreeId,
                    TreeParentId = permissionKey.TreeParentId,
                    PermissionAccessType = currentUserGroupPermission == null ? PermissionAccessTypes.None : currentUserGroupPermission.PermissionAccessType,
                });
            }

            UserGroupPermissions.AddRange(allUserGroupPermissions);
        }

        private void RefreshUserGroupPermissionsOnCurrentObject()
        {
            if (CurrentObject == null) return;

            foreach (var userGroupPermission in UserGroupPermissions)
            {
                var orgUserGroupPermission = CurrentObject.UserGroupPermissions.FirstOrDefault(ugp => ugp.PermissionKey.PermissionKeyId == userGroupPermission.PermissionKey.PermissionKeyId);
                orgUserGroupPermission.PermissionAccessType = userGroupPermission.PermissionAccessType;
            }
        }
    }
}